﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Program
{
    class App
    {
        static async Task Main(string[] args)
        {
            DateTime utcDate = DateTime.UtcNow;

            try
            {
                string workout = RandomGenerator.GenerateWorkout();
                Console.WriteLine($"[{utcDate}]: Workout Successfully Generated, Sending Email...");

                await Email.SendEmail(workout);
                Console.WriteLine($"[{utcDate}]: Email Sent Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error [{utcDate}]: {ex.Message}");
            }

        }

    }

    public class RandomGenerator
    {        
        private static Schema SelectWorkoutSchema()
        {
            WorkoutData data = Workout.ReadData();
            Random random = new Random();
            int index = random.Next(data.Schemas.Count);

            return data.Schemas[index];
        }

        private static string DetermineTimeLimit()
        {
            Random random = new Random();
            int[] timeValues = [ 10, 12, 15, 18, 20, 21 ];
            int index = random.Next(timeValues.Length);
            string timeLimit = timeValues[index].ToString();

            return timeLimit;
        }

        private static string DetermineReps()
        {
            Random random = new Random();
            int[] repValues = [ 5, 6, 10, 12, 15, 18, 20, 21, 25, 30 ];
            int index = random.Next(repValues.Length);
            string reps = repValues[index].ToString();

            return reps;
        }

        private static List<string> SelectExercises()
        {
            Random random = new Random();
            WorkoutData data = Workout.ReadData();
            List<Exercise> availableExercises = new(data.Exercises);

            int noExercises = random.Next(2,5);
            noExercises = Math.Min(noExercises, availableExercises.Count);
            
            List<string> selectedExercises = new List<string>();

            for (int i = 0; i < noExercises; i++)
            {
                int index = random.Next(availableExercises.Count);
                selectedExercises.Add(availableExercises[index].Name);
                availableExercises.RemoveAt(index);
            }

            return selectedExercises;
        }

        public static string GenerateWorkout()
        {
            Schema schema = SelectWorkoutSchema();
            List<string> exercises = SelectExercises();
            string timeLimit = schema.Time ? DetermineTimeLimit() : "";
            string reps = schema.Reps ? DetermineReps() : "";

            string workout = "Workout of the Day\n";
                workout += $"{schema.Name}\n";
                workout += $"{schema.Description}\n";
                if (!string.IsNullOrEmpty(timeLimit))
                    workout += $"Time: {timeLimit} minutes\n";
                foreach (var exercise in exercises)
                {
                    if (!string.IsNullOrEmpty(reps))
                        workout += $"- {reps} x {exercise}\n";
                    else
                        workout += $"- {exercise}\n";
                }

                return workout ;

        }
    }

    public class Workout 
    {
        public static WorkoutData ReadData()
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string jsonPath = Path.Combine(exePath, "data.json");
            string json = File.ReadAllText(jsonPath);

            return JsonSerializer.Deserialize<WorkoutData>(json)!;
        }
    }

    public class Email
    {
        public static async Task SendEmail( string body )
        {
            try {
                IConfigurationRoot config = new ConfigurationBuilder()
                    .AddEnvironmentVariables()
                    .Build();

                DateTime today = DateTime.Today;

                string subject = $"Workout of the Day - {today:D}";
                string toEmail = "YOUR_EMAIL_ADDRESS";

                string fromEmail = config["EMAIL_ADDRESS"] ?? throw new InvalidOperationException("Email address not found in configuration");
                string smtpPassword = config["EMAIL_PASSWORD"] ?? throw new InvalidOperationException("SMTP password not found in configuration");

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromEmail, smtpPassword),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage(fromEmail, toEmail, subject, body);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"SMTP error occured: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while sending the email: {ex.Message}");
                throw;
            }
            
        }
    }


    public class WorkoutData
    {
        [JsonPropertyName("exercises")]
        public required List<Exercise> Exercises { get; set; }
        [JsonPropertyName("schemas")]
        public required List<Schema> Schemas { get; set; }
    }

    public class Exercise
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }
        [JsonPropertyName("equipment")]
        public required string Equipment { get; set; }
    }

    public class Schema
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }
        [JsonPropertyName("description")]
        public required string Description { get; set; }
        [JsonPropertyName("time")]
        public required bool Time { get; set; }
        [JsonPropertyName("reps")]
        public required bool Reps { get; set; }
    }
}