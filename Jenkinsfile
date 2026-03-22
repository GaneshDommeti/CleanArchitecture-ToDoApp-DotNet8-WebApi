pipeline {
    agent any

    environment {
        AWS_REGION = 'us-east-1'
        FUNCTION_NAME = 'TodoFunction'
        PROJECT_PATH = 'TodoApp.WebAPI'
        ASSEMBLY_NAME = 'TodoApp.WebAPI' 
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main',
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
                // Pointing to the specific solution file
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
            steps {
                dir("${env.PROJECT_PATH}") {
                    bat "dotnet lambda package --configuration Release --output-package ../deploy.zip"
                }
            }
        }

        stage('Deploy') {
            steps {
                withCredentials([[
                    $class: 'AmazonWebServicesCredentialsBinding', 
                    credentialsId: 'aws-credentials'
                ]]) {
                    bat """
                    aws lambda update-function-code ^
                    --function-name %FUNCTION_NAME% ^
                    --zip-file fileb://deploy.zip ^
                    --region %AWS_REGION%
                    """
                    bat """
                    aws lambda update-function-configuration ^
                    --function-name %FUNCTION_NAME% ^
                    --handler %ASSEMBLY_NAME% ^
                    --region %AWS_REGION%
                    """
                }
            }
        }
    }
    post {
        success {
            echo "Deployment successful! Todo API is live."
        }
        failure {
            echo "Deployment failed. Check the logs above."
        }
    }
}
