//
// Copyright 2014-2015 Amazon.com, 
// Inc. or its affiliates. All Rights Reserved.
// 
// Licensed under the AWS Mobile SDK For Unity 
// Sample Application License Agreement (the "License"). 
// You may not use this file except in compliance with the 
// License. A copy of the License is located 
// in the "license" file accompanying this file. This file is 
// distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, express or implied. See the License 
// for the specific language governing permissions and 
// limitations under the License.
//

using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.IO;
using Amazon;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;

public class LeaderboardDriver : DynamoDbBase
{
    private static LeaderboardDriver instance;

    public static LeaderboardDriver Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<LeaderboardDriver>();
            }

            return instance;
        }
    }

    private static IAmazonDynamoDB _client;
    private static DynamoDBContext _context;
    private static bool table_readable = false;
    private static List<Dictionary<string, AttributeValue>> result_set = null;
    private static string current_id = null;
    private static string current_name = null;

    private DynamoDBContext Context
    {
        get
        {
            if (_context == null)
                _context = new DynamoDBContext(_client);

            return _context;
        }
    }

    public static bool Readable
    {
        get
        {
            return table_readable;
        }
    }

    public static List<Dictionary<string, AttributeValue>> Results
    {
        get
        {
            return result_set;
        }
    }

    public static string Name
    {
        get
        {
            return current_name;
        }
        set
        {
            StreamWriter cache = new StreamWriter("./.IcarusCache/name.blob");
            cache.Write(value);
            cache.Close();

            current_name = value;
        }
    }

    public static string Id
    {
        get
        {
            return current_id;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        UnityInitializer.AttachToGameObject(this.gameObject);
        Amazon.AWSConfigs.HttpClient = Amazon.AWSConfigs.HttpClientOption.UnityWebRequest;

        string id_path = "./.IcarusCache/id.blob";
        if (!Directory.Exists(".IcarusCache"))
        {
            Directory.CreateDirectory(".IcarusCache");
            StreamWriter cache = new StreamWriter(id_path);
            System.Random rand = new System.Random();
            cache.Write(Encrypt.EncryptString(rand.Next(1000000000).ToString("D10"), "LazyUnlock1"));
       
            cache.Close();
        }
        StreamReader reader = new StreamReader(id_path);
        current_id = Encrypt.DecryptString(reader.ReadToEnd(), "LazyUnlock1");

        string name_path = "./.IcarusCache/name.blob";
        if (File.Exists(name_path))
        {
            reader = new StreamReader(name_path);
            current_name = reader.ReadToEnd();
        }

        _client = Client;
        //PerformCreateOperation(1, this.current_id, "Chris", 5000);
    }

    private void PerformCreateOperation(int in_level, string in_id, string in_name, int in_score)
    {
        LevelScore myLevelScore = new LevelScore
        {
            Id = in_id,
            Level = in_level,
            Name = in_name,
            Score = in_score
        };

        Context.SaveAsync(myLevelScore, (result) => { Debug.Log(result.Exception.Message); });

        Debug.Log("Saved new score");
    }

    public static void FindScoresForLevel(int level)
    {
        FindScoresHelper(new QueryRequest(), level, null);
    }

    static void FindScoresHelper(QueryRequest request, int level, Dictionary<string, AttributeValue> lastKeyEvaluated)
    {
        table_readable = false;

        request.TableName = "IcarusLeaderboard";
        request.IndexName = "LevelHigh-Index";

        request.KeyConditions = new Dictionary<string, Condition>()
        {
            {
                "Level",  new Condition()
                {
                    ComparisonOperator = "EQ",
                    AttributeValueList = new List<AttributeValue>()
                    {
                        new AttributeValue { N = level.ToString() }
                    }
                }
            },
            {
                "Score", new Condition()
                {
                    ComparisonOperator = "LT",
                    AttributeValueList = new List<AttributeValue>()
                    {
                        new AttributeValue { N = int.MaxValue.ToString() }
                    }
                }
            }
        };

        request.ConsistentRead = false;
        request.Limit = 10;
        request.ExclusiveStartKey = lastKeyEvaluated;
        request.ScanIndexForward = false;

        _client.QueryAsync(request, (result) => {
            result_set = result.Response.Items;
            table_readable = true;
        });
    }

    [DynamoDBTable("IcarusLeaderboard")]
    public class LevelScore
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        [DynamoDBProperty]
        public int Level { get; set; }
        [DynamoDBProperty]
        public string Name { get; set; }
        [DynamoDBProperty]
        public int Score { get; set; }
    }
}
