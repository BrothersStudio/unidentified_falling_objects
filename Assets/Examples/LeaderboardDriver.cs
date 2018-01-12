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

    private IAmazonDynamoDB _client;
    private DynamoDBContext _context;
    private bool table_readable = false;
    private List<Dictionary<string, AttributeValue>> result_set = null;
    private string current_id = null;

    private DynamoDBContext Context
    {
        get
        {
            if (_context == null)
                _context = new DynamoDBContext(_client);

            return _context;
        }
    }

    private void Update()
    {
        if (table_readable)
        {
            Debug.Log("result set available");
            Debug.Log(result_set);

            foreach (var item in result_set)
            {
                Debug.Log(item["Score"].N + " " + item["Name"].N);
            }
        }
    }

    void Awake()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        Amazon.AWSConfigs.HttpClient = Amazon.AWSConfigs.HttpClientOption.UnityWebRequest;

        string path = "./.IcarusCache/cache.blob";
        if (!Directory.Exists(".IcarusCache"))
        {
            Directory.CreateDirectory(".IcarusCache");
            StreamWriter cache = new StreamWriter(path);
            System.Random rand = new System.Random();
            cache.Write(Encrypt.EncryptString(rand.Next(1000000000).ToString("D10"), "LazyUnlock1"));
       
            cache.Close();
        }

        StreamReader reader = new StreamReader(path);
        this.current_id = Encrypt.DecryptString(reader.ReadToEnd(), "LazyUnlock1");

        _client = Client;
        PerformCreateOperation(1, this.current_id, "Chris", 5000);
        FindScoresForLevel(1);
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

    void FindScoresForLevel(int level)
    {
        FindScoresHelper(new QueryRequest(), level, null);
    }

    void FindScoresHelper(QueryRequest request, int level, Dictionary<string, AttributeValue> lastKeyEvaluated)
    {
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
            this.table_readable = true;
        });
    }
    //private void PerformUpdateOperation()
    //{
    //    // Retrieve the book. 
    //    Book bookRetrieved = null;
    //    Context.LoadAsync<Book>(bookID,(result)=>
    //    {
    //        if(result.Exception == null )
    //        {
    //            bookRetrieved = result.Result as Book;
    //            // Update few properties.
    //            bookRetrieved.ISBN = "222-2222221001";
    //            bookRetrieved.BookAuthors = new List<string> { " Author 1", "Author x" }; // Replace existing authors list with this.
    //            Context.SaveAsync<Book>(bookRetrieved,(res)=>
    //            {
    //                if(res.Exception == null)
    //                    resultText.text += ("\nBook updated");
    //            });
    //        }
    //    });
    //}

    [DynamoDBTable("IcarusLeaderboard")]
    public class LevelScore
    {
        [DynamoDBHashKey]   // Hash key.
        public string Id { get; set; }
        [DynamoDBProperty]
        public int Level { get; set; }
        [DynamoDBProperty]
        public string Name { get; set; }
        [DynamoDBProperty]
        public int Score { get; set; }
    }
}
