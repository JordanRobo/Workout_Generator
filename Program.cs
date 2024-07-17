using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Program
{
    class App
    {
        static void Main(string[] args)
        {
            static bool TimeCheck()
            {
                return true;
            }

            if (TimeCheck())
            {
                string workout = RandomGenerator.GenerateWorkout();
                Console.WriteLine(workout);
                // Add in Email.SendEmail(workout); method here once ready
            } 
            else
            {
                Console.WriteLine("The Time is not 5am AEST");
            }
        }

    }

    public class RandomGenerator
    {        
        public static Schema SelectWorkoutSchema()
        {
            WorkoutData data = Workout.ReadData();
            Random random = new Random();
            int index = random.Next(data.Schemas.Count);

            return data.Schemas[index];
        }

        public static string DetermineTimeLimit()
        {
            Random random = new Random();
            int[] timeValues = [ 10, 12, 15, 18, 20, 21 ];
            int index = random.Next(timeValues.Length);
            string timeLimit = timeValues[index].ToString();

            return timeLimit;
        }

        public static string DetermineReps()
        {
            Random random = new Random();
            int[] repValues = [ 5, 6, 10, 12, 15, 18, 20, 21, 25, 30 ];
            int index = random.Next(repValues.Length);
            string reps = repValues[index].ToString();

            return reps;
        }

        public static List<string> SelectExercises()
        {
            Random random = new Random();
            int noExercises = random.Next(2,5);

            WorkoutData data = Workout.ReadData();
            List<string> selectedExercises = new List<string>();

            for (int i =0; i < noExercises; i++)
            {
                int index = random.Next(data.Exercises.Count);
                selectedExercises.Add(data.Exercises[index].Name);
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
        public static void SendEmail()
        {
            Console.WriteLine("Send email to an address containing the Workout of the Day");
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