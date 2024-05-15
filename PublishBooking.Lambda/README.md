# Publish Booking Lambda

This Lambda is triggered by a request from the Api Gateway.

This project was created with the following command: 
```
dotnet new serverless.EmptyServerless --name PublishBooking.Lambda
```

## Decisions made
* The Lambda is called PublishBooking rather than PublishBooking.Lambda. This is because the `dotnet lambda deploy-serverless` command generates a lot of objects within AWS and uses the lambda name as a base. There was an error where something couldn't be created because the name wasn't alphanumeric. I renamed the lambda function as a way to avoid this.

## TODO / Tech debt
* Validation of the event message details
* Error handling, especially around publishing to sns
* Production and dev config - we need to specify the SNS arn to publish to
* Directory cleanup? We can probably remove the `PublishBooking.Lambda/PublishBooking/` directory and move the `src` and `test` directories within `PublishBooking.Lambda/`. That directory was created by the `dotnet new serverless.EmptyServerless` command and I think it's not needed. 
* Not sure we actually need steps 1 and 2 of the environment setup now that its using serverless, i think it creates its own roles

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
9. Create a method against your resource: Create method -> method type: POST, integration type: lambda function, Lambda function: arn of your lambda function
10. Test - In API Gateway under your created method there is a Test button, you can use this to ensure your api gateway is calling your lambda. You should get a 202 response back in api-gateway. You can also go into your lambda and checkout the cloudwatch logs.
