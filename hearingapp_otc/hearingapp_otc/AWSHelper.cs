

//using System;
//using System.Collections.Generic;
//using System.Text;
//using Amazon.Runtime;
//using Amazon.CognitoIdentityProvider;
//using Amazon.Extensions.CognitoAuthentication;
//using Amazon.CognitoIdentity;
//using Amazon;
//using Amazon.DynamoDBv2;
//using Amazon.DynamoDBv2.DataModel;
//using System.Threading.Tasks;
//using hearingapp_otc.Classes;

//namespace hearingapp_otc
//{
//    public class AWSHelper
//    {
//        public static CognitoUser regUser;
//        public static AuthFlowResponse AWSAuthFlowResponseContext;
//        public static DynamoDBContext dynamoContext;

//        public static string CognitoIdentityPoolId = "us-east-1:45b84862-bcb9-4405-8653-b1c25387a6c4";

//        public AWSHelper()
//        {

//        }

//        // STORE AND RETRIEVE DATA FROM DYNAMO
//        // REF: https://docs.aws.amazon.com/mobile/sdkforxamarin/developerguide/getting-started-store-retrieve-data.html

//        //public static async void AttemptLogin(string username, string password)
//        public static async Task<bool> AttemptLogin(string username, string password)
//        {

//            var provider = new AmazonCognitoIdentityProviderClient(new AnonymousAWSCredentials(),
//                                                                   FallbackRegionFactory.GetRegionEndpoint());

//            CognitoUserPool userPool = new CognitoUserPool("us-east-1_f78X24m3R", "3pg1jr137e0d49i3hkfjdm5157", provider);

//            regUser = new CognitoUser(username,
//                                        "3pg1jr137e0d49i3hkfjdm5157",
//                                        userPool,
//                                        provider,
//                                        "lu6sj2o5qudejknanssu3976maug9upudsgn573h04vlv8neof3");

//            try
//            {
//                AWSAuthFlowResponseContext = await regUser.StartWithSrpAuthAsync(new InitiateSrpAuthRequest()
//                {
//                    Password = password
//                }).ConfigureAwait(false);

//            }
//            // Throws an exception when a bad username/password pair is provided
//            catch (Amazon.CognitoIdentityProvider.Model.NotAuthorizedException e)
//            {
//                //Console.WriteLine("Bad creds - " + e.ToString());
//                Console.WriteLine("AWSHelper::AttemptLogin - Bad username or password provided :(");
//                return false;
//            }

//            catch (Amazon.CognitoIdentityProvider.Model.InvalidParameterException e)
//            {
//                Console.WriteLine("AWSHelper::AttemptLogin - Missing either username or password :(");
//                return false;
//            }

//            catch (Amazon.CognitoIdentityProvider.Model.UserNotFoundException e)
//            {
//                Console.WriteLine("AWSHelper::AttemptLogin - Username provided does not exist :(");
//                return false;
//            }

//            // It worked? Return success
//            Console.WriteLine("AWSHelper::AttemptLogin - Login successful! Yay! :)");
//            return true;

//        }

//        public static async void RegisterNewSession(Session newUserInfo)
//        {
//            CognitoAWSCredentials credentials = regUser.GetCognitoAWSCredentials(CognitoIdentityPoolId, RegionEndpoint.USEast1);
//            //credentials.ClearCredentials();
//            //credentials.Clear();
//            var client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast1);
//            dynamoContext = new DynamoDBContext(client);

//            await dynamoContext.SaveAsync(newUserInfo);
//        }

//        public static async void PerformSessionUpdate(Session existingSession)
//        {
//            CognitoAWSCredentials credentials = regUser.GetCognitoAWSCredentials(CognitoIdentityPoolId, RegionEndpoint.USEast1);
//            var client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast1);
//            dynamoContext = new DynamoDBContext(client);

//            await dynamoContext.SaveAsync(existingSession);
//        }




//        //
//        // TESTING - GET RID OF THIS CRAP AT SOME POINT
//        //
//        public static async void TryDynamoDBStuff()
//        {
//            CognitoAWSCredentials credentials = regUser.GetCognitoAWSCredentials(CognitoIdentityPoolId, RegionEndpoint.USEast1);
//            var client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast1);
//            dynamoContext = new DynamoDBContext(client);

//            // Test: read
//            //AWSUserInfo retrieveInfo = await dynamoContext.LoadAsync<AWSUserInfo>("00000000-0000-0000-0000-000000000000");
//            Session retrieveInfo = await dynamoContext.LoadAsync<Session>("00000000-0000-0000-0000-000000000000");

//            // Test: insert (save)
//            Session newUserInfo = new Session()
//            {
//                FirstName = "BarnabyJones"
//            };
//            await dynamoContext.SaveAsync(newUserInfo);

//            // Test: update
//            newUserInfo.FirstName = "LeeroyJones";
//            await dynamoContext.SaveAsync(newUserInfo);

//            /*DELETE
//            Book retrievedBook = context.Load<Book>(1);
//            context.Delete(retrievedBook);
//            */
//            return;
//        }

//        public static async void ClearCognitoCreds()
//        {
//            CognitoAWSCredentials credentials = regUser.GetCognitoAWSCredentials(CognitoIdentityPoolId, RegionEndpoint.USEast1);
//            credentials.Clear();
//            return;
//        }

//    }

//}
