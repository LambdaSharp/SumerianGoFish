# Sumerian GoFish Challenge

Can you beat the Sumerian host at GoFish?

## About this challenge

Using Amazon Sumerian, Amazon Lex, AWS Lambda, and Amazon DynamoDB, we’ll create a Sumerian scene as a visual interface for the game.

### How to play

2 players, 26 cards total. 7 cards for each player to start. Take turns asking for the drink on the card. Try to match two of the same drink card to get two points. Each player can only ask for one card per turn.
 
Drink cards:

- Amaretto Sour
- Bloody Mary
- Cosmopolitan
- Long Island Ice Tea
- Mai Tai
- Margarita
- Martini
- Mojito
- Moscow Mule
- Old Fashioned
- Pina Colada
- Whiskey Sour
- White Russian 

### AWS Services

- Must use AWS region **`us-east-1`**
- [Amazon DynamoDB](https://aws.amazon.com/dynamodb/)
- [AWS Lambda](https://aws.amazon.com/lambda/)
- [AWS CloudFormation](https://aws.amazon.com/cloudformation/)
- [Amazon Lex](https://aws.amazon.com/lex/)
- [Amazon Sumerian](https://aws.amazon.com/sumerian/)
- [Amazon Cognito](https://aws.amazon.com/cognito/)

## Setup

<details>
<summary>Setup Instructions</summary>

---

- [Install .Net 2.1](https://dotnet.microsoft.com/download)
- [Sign up for AWS account](https://aws.amazon.com/)
- [Install AWS CLI](https://docs.aws.amazon.com/cli/latest/userguide/cli-chap-install.html)
- [See Latest Release Notes for Sumerian Browser Compatibility](https://aws.amazon.com/releasenotes/?tag=releasenotes%23keywords%23amazon-sumerian)
- Must use AWS region `us-east-1`
- Setup - LambdaSharp Tool (aka lash)

  - Upgrade lash

	```
	dotnet tool uninstall -g LambdaSharp.Tool
	dotnet tool install -g LambdaSharp.Tool --version 0.7.0-rc7
	lash --version
	```
	OR
  - Install lash

	```
	dotnet tool install -g LambdaSharp.Tool --version 0.7.0-rc7
	lash --version
	```

- Initialize a LambdaSharp environment

	```
	lash init --quick-start
	```
