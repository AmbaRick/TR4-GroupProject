# Publish Booking Lambda

This Lambda is triggered by a request from the Api Gateway.

This project was created with the following command: 
```
dotnet new serverless.EmptyServerless --name PublishBooking.Lambda
```

## To set up your environment
1. Create IAM Policy called `lambda-apigateway-policy` with the following policy: 
```{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Sid": "Stmt1428341300017",
      "Action": [
        "dynamodb:DeleteItem",
        "dynamodb:GetItem",
        "dynamodb:PutItem",
        "dynamodb:Query",
        "dynamodb:Scan",
        "dynamodb:UpdateItem"
      ],
      "Effect": "Allow",
      "Resource": "*"
    },
    {
      "Sid": "",
      "Resource": "*",
      "Action": [
        "logs:CreateLogGroup",
        "logs:CreateLogStream",
        "logs:PutLogEvents"
      ],
      "Effect": "Allow"
    }
  ]
}
```

2. Create IAM role called `lambda-apigateway-role`
   trusted entity: AWS service, use-case: lambda, policy: `lambda-apigateway-policy`. copy the arn, you’ll need this later 
3. Create an S3 bucket to store the output.
4. Run your tests cd "PublishBooking/test/PublishBooking.Tests" 
```
dotnet test
```
5. Deploy the function. Set a stack name and enter the s3 bucket name you just created
```dotnet lambda deploy-serverless PublishBooking 
cd <YOUR PATH>/PublishBooking.Lambda/src/PublishBooking.Lambda
dotnet lambda deploy-serverless --stack-name PublishBookingStack --s3-bucket <YOUR S3 BUCKET NAME>
```
You should be able to see your function in the AWS console under lambdas, take note of the arn - you’ll need it when setting up the api gateway
7. Invoke the function:
```
aws lambda invoke --function-name <YOUR LAMBDA FUNCTION NAME> --payload "" response.json
```
8. Create the API Gateway. In the AWS API Gateway console, Create API -> Rest API -> build -> New Api called PublishBookingOperations.  create a resource with the name EventBookingManager
9. Create a resource with the name EventBookingManager against your API
9. Create a method against your resource: Create method -> method type: GET, integration type: lambda function, Lambda function: arn of your lambda function
10. Test - In API Gateway under your created method there is a Test button, you can use this to ensure your api gateway is calling your lambda. You should get a 200 response back in api-gateway. You can also go into your lambda and checkout the cloudwatch logs.
