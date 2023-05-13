import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;
import java.util.TimeZone;
pipeline {
    agent any
    
    parameters {
        string (
                name:'BRANCH',
                defaultValue: 'develop',
                description: 'Branch git'
            )
    }
    stages {
        stage('Start Build') {
            steps {
                script {
                    def currentDateTime = getCurrentDateTime()
                    def user = getCurrentUserInfo().userId
                    // change display build name and description
                    currentBuild.displayName = "#${BUILD_NUMBER}-${BRANCH}"
                    currentBuild.description = "#Deploy by ${user} on ${currentDateTime}"
                }
            }
        }
    }
}
node {
    stage('Pull Source Code') {
        git branch: "${params.BRANCH}",
            credentialsId: "jenkins",
            url: "git@bitbucket.org:dtranthai/fashion-api.git"
    }

    stage('Running Restore Project Package') {
        sh "dotnet restore"
    }

    stage('Running Build Project') {
        sh "dotnet build --no-restore --configuration Release"
    }

    stage('Running Unit Test') {
        sh "dotnet test --no-restore --no-build --configuration Release -v d"
    }

    stage('Publish Source') {
        sh "dotnet publish --no-restore --no-build -c Release API/API.csproj -o publish"
    }

    def timeCreateFolder = getCurrentDateTime("dd.MM.yyy")
    def deploy_folder = "/media/source/api/api-${timeCreateFolder}-${BUILD_NUMBER}"

    stage('Deploying Server') {
        withCredentials([usernamePassword(credentialsId: 'api-server', usernameVariable: 'userName', passwordVariable: 'password')]) {
            def server = [:]
            server.name = "api-server"
            server.host = "192.168.50.44"
            server.allowAnyHosts = true
            server.user = userName
            server.password = password

            stage("Create Deploy Folder") {
                sshCommand remote: server, command: "echo Create deploy folder"
                sshCommand remote: server, command: "mkdir -p ${deploy_folder}"
            }

            stage("Send file to deploy folder") {
                sshPut remote: server, from: "publish/", into: "${deploy_folder}"
                sshCommand remote: server, command: "mv ${deploy_folder}/publish/* ${deploy_folder}"
                sshCommand remote: server, command: "rm -rf ${deploy_folder}/publish"
            }

            stage("Running server") {
                sshCommand remote: server, command: "dotnet ${deploy_folder}/API.dll"
            }
        }
    }
}
def getCurrentDateTime(String typeFormatDate = "EEEE, MMM d, yyyy, h:mm:ss a") {
    def formatDate = new SimpleDateFormat(typeFormatDate, Locale.getDefault())
    formatDate.setTimeZone(TimeZone.getDefault())
    def date = new Date()
    def timeFormat = formatDate.format(date)
    return timeFormat
}
def getCurrentUserInfo() {
    def userCurrent = [:]
    wrap([$class: 'BuildUser']) {
        userCurrent.fullName = "${BUILD_USER}"
        userCurrent.firstName = "${BUILD_USER_FIRST_NAME}"
        userCurrent.userId = "${BUILD_USER_ID}"
        userCurrent.email = "${BUILD_USER_EMAIL}"
    }
    return userCurrent
}