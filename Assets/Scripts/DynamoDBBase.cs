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

using UnityEngine;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon;
using System;

public class DynamoDbBase : MonoBehaviour
{
    public static string DynamoRegion = RegionEndpoint.USEast1.SystemName;

    private static RegionEndpoint _DynamoRegion
    {
        get { return RegionEndpoint.GetBySystemName(DynamoRegion); }
    }

    private static IAmazonDynamoDB _ddbClient;

    private static AWSCredentials _credentials;

    private static AWSCredentials Credentials
    {
        get
        {
            try
            {
                if (_credentials == null)
                {
                    TextAsset key = Resources.Load("key") as TextAsset;
                    string[] split_key = key.text.Split(',');
                    _credentials = new BasicAWSCredentials(split_key[0], split_key[1]);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

            return _credentials;
        }
    }

    protected static IAmazonDynamoDB Client
    {
        get
        {
            try
            {
                if (_ddbClient == null)
                {
                    _ddbClient = new AmazonDynamoDBClient(Credentials, _DynamoRegion);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            return _ddbClient;
        }
    }

}
