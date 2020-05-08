# What am I trying to achieve?

### Two Objectives:
1. Cross Domain Cookie sharing in asp.net core to achieve SSO like functionality
2. Containerizing all three apps (authentication, and two sample client apps) using Docker

### Please follow the steps below:
1. Make sure your docker engine is running (to install Doker, go to https://docs.docker.com/engine/install/)
2. Follow the steps to run all containers:
```
git clone https://github.com/vdonthireddy/dotnet-core-sharing-cookie-auth.git
cd dotnet-core-sharing-cookie-auth/
cd sharing-cookie-auth
bash allrun.sh
```