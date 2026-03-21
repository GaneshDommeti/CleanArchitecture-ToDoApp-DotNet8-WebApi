pipeline {
    agent any

    environment {
        AWS_REGION = 'us-east-1'
        FUNCTION_NAME = 'TodoFunction'
    }

    stages {

        stage('Restore') {
    steps {
        bat 'dotnet restore TodoApp.WebAPI\\TodoApp.WebAPI.sln'
    }
}

        stage('Build') {
    steps {
        bat 'dotnet build TodoApp.WebAPI\\TodoApp.WebAPI.sln --configuration Release'
    }
}

        stage('Test') {
            steps {
                 bat 'dotnet test TodoApp.WebAPI\\TodoApp.WebAPI.sln'
            }
        }

        stage('Install Lambda Tools') {
            steps {
                bat 'dotnet tool install -g Amazon.Lambda.Tools || echo already installed'
            }
        }
        
stage('Publish') {
    steps {
        bat '''
        dotnet publish TodoApp.WebAPI\\TodoApp.WebAPI.csproj ^
        -c Release ^
        -o publish
        '''
    }
}

stage('Package Lambda') {
    steps {
        bat '''
        dotnet lambda package ^
        --project-location TodoApp.WebAPI ^
        --configuration Release ^
        --output-package deploy.zip
        '''
    }
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
