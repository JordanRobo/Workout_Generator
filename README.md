# 🏋️‍♂️ Workout Generator

## Because planning workouts is harder than doing them

Ever found yourself wanting to get fit but struggling with the trifecta of workout woes?

1. Gym memberships cost more than your coffee addiction
2. Planning workouts feels like rocket science
3. Routine is the enemy of motivation

Well, fear not! I've created this incredible C# console app to solve all these problems and more. Welcome to the Workout Generator, where fitness meets innovation.

## Features

- Generates a "Workout of the Day" faster than you can say "I'll start on Monday"
- Emails your workout at 5am, because nothing says "good morning" like burpees
- Customisable exercises and workout schemas via a `data.json` file
- Hosted on DigitalOcean's Droplets, because even apps need a good home

## How It Works

1. The app wakes up at 5am, earlier than any of us want to
2. It randomly selects a workout schema and exercises from `data.json`
3. It crafts an email containing the workout plan
4. The email arrives in your inbox, ready to inspire (or terrify) you when you wake up


## Setup

These instructions assume that you have the necessary tools installed on your local Linux machine (git, dotnet SDK) and that you have a DigitalOcean account set up.

Remember to replace `droplet_ip`, `from_email@example.com`, `to_email@example.com`, `smtp_port`, `smtp_address` and `from_email_password` with your actual values. 

*(Note: I used Google so if you are too use `smtp_address = smtp.gmail.com` and `smtp_port = 587`)*

<details>
<summary><h3>Instructions for Linux Users</h3></summary>

1. Clone the repository:
   ```bash
   git clone https://github.com/JordanRobo/Workout_Generator.git
   ```

2. Navigate to the cloned directory:
   ```bash
   cd Workout_Generator
   ```

3. Publish the application:
   ```bash
   dotnet publish -c Release -r linux-x64 --self-contained
   ```

4. Create a deployment package:
   ```bash
   mkdir -p deployment-package
   cp -r bin/Release/net8.0/linux-x64/publish/* deployment-package/
   ```

### On DigitalOcean

5. Create a new droplet on DigitalOcean through the web interface or using doctl.

6. SSH into your new droplet:
   ```bash
   ssh root@droplet_ip
   ```

7. Update the system and install .NET runtime:
   ```bash
   sudo apt update
   sudo apt install -y aspnetcore-runtime-8.0
   ```

8. Create a directory for your application:
   ```bash
   mkdir -p /opt/workout-generator
   ```

9. Exit the droplet SSH session.

### Transferring Files

10. From your local machine, transfer your application files to the droplet:
    ```bash
    scp -r deployment-package/* root@your_droplet_ip:/opt/workout-generator/
    ```

### Final Configuration on the Droplet

11. SSH back into your droplet:
    ```bash
    ssh root@your_droplet_ip
    ```

12. Set up environment variables (replace with your actual email credentials):
    ```bash
    echo 'export EMAIL_TO_ADDRESS="to_email@example.com"' >> /root/.profile
    echo 'export SMTP_ADDRESS="smtp_address"' >> /root/.profile
    echo 'export SMTP_PORT="smtp_port"' >> /root/.profile
    echo 'export EMAIL_FROM_ADDRESS="from_email@example.com"' >> /root/.profile
    echo 'export EMAIL_FROM_PASSWORD="from_email_password"' >> /root/.profile
    source /root/.profile
    ```

13. Set the timezone (Replace `TIMEZONE/REGION` with your desired region):
    ```bash
    sudo timedatectl set-timezone TIMEZONE/REGION
    ```

14. Set up a cron job to run your application daily at 5am:
    ```bash
    (crontab -l 2>/dev/null; echo "0 5 * * * cd /opt/workout-generator && ./Workout_Generator") | crontab -
    ```

15. Verify the cron job:
    ```bash
    crontab -l
    ```

16. Test run your application:
    ```bash
    cd /opt/workout-generator
    ./Workout_Generator
    ```
</details>

<details>
<summary><h3>Maintaining Your Application</h3></summary>

- To modify exercises, SSH into your droplet and edit the data file:
  ```bash
  nano /opt/workout-generator/data.json
  ```
  
- Remember to maintain proper JSON syntax when editing.

- After making changes, you can manually run the application to test:
  ```bash
  cd /opt/workout-generator && ./Workout_Generator
  ```

  > Tip: to make it easier to edit create a bash alias.
</details>

<details>
<summary><h3>Troubleshooting</h3></summary>

- Check application logs (if implemented) for any errors.
- Verify that the cron job is running by checking system logs:
  ```bash
  grep CRON /var/log/syslog
  ```
- Ensure environment variables are set correctly in /root/.profile.
</details>


### About the Developer

I'm just a guy who decided that if computers can solve complex algorithms, they can surely tell me how many push-ups to do. When I'm not coding, I'm probably thinking about coding, or doing a workout that this app told me to do.

Remember, the only bad workout is the one that didn't happen. So let this app be your digital fitness buddy, personal trainer, and morning alarm all rolled into one. Happy sweating!
