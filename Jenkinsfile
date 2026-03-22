pipeline {
    agent any

    environment {
        AWS_REGION = 'us-east-1'
        FUNCTION_NAME = 'TodoFunction'
    }

    stages {

        stage('Checkout') {
            steps {
                git branch: 'master',
                    url: 'https://github.com/GaneshDommeti/CleanArchitecture-ToDoApp-DotNet8-WebApi.git',
                    credentialsId: 'github-creds'
            }
        }
        stage('Setup Tools') {
            steps {
               bat 'dotnet tool install -g Amazon.Lambda.Tools || echo already installed'
            }
        }
        
        stage('Test') {
            steps {
                bat 'dotnet test TodoApp.WebAPI\\TodoApp.WebAPI.sln'
            }
        }
        
        stage('Restore & Build') {
            steps {
                bat 'dotnet restore'
                bat 'dotnet build --configuration Release'
            }
        }
              
        stage('Package Lambda') {
           bat 'dotnet lambda package --configuration Release --output-package deploy.zip'
        }

        stage('Deploy') {
            steps {
                withCredentials([[
                    $class: 'AmazonWebServicesCredentialsBinding',
                    credentialsId: 'aws-credentials'
                ]]) {
                    bat '''
                    aws lambda update-function-code ^
                    --function-name %FUNCTION_NAME% ^
                    --zip-file fileb://deploy.zip ^
                    --region %AWS_REGION%
                    '''
                }
            }
        }
    }
}
