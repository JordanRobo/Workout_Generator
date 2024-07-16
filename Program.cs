using System;
using System.Text.Json;

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

            if (TimeCheck() == true)
            {
                RandomGenerator.SelectWorkoutSchema();
            } else
            {
                Console.WriteLine("The Time is not 5am AEST");
            }
        }

    }

    public class RandomGenerator
    {        
        public static string SelectWorkoutSchema()
        {
            string selectedSchema = "Workout Schema";

            return selectedSchema;
        }

        public static int DetermineTimeLimit()
        {
            int timeLimit = 10;

            return timeLimit;
        }

        public static int DetermineReps()
        {
            int reps = 10;

            return reps;
        }

        public static string SelectExcercises()
        {
            string selectedExcercises = "Excercise 1, Excercise2";

            return selectedExcercises;
        }

        public static string GenerateWorkout()
        {
            string generatedWorkout = SelectWorkoutSchema();

            return generatedWorkout;
        }
    }

    public class Workout 
    {
        public static WorkoutData ReadData()
        {
            string json = File.ReadAllText("data.json");
            return JsonSerializer.Deserialize<WorkoutData>(json);
        }
    }

    public class WorkoutData
    {
        public List<Exercise> Excercises { get; set; }
        public List<Schema> Schemas { get; set; }
    }

    public class Exercise
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Schema
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Time { get; set; }
        public bool Reps { get; set; }
    }

    public class Email 
    {
        public static void SendEmail() 
        {
            Console.WriteLine("Send email to an address containing the Workout of the Day");
        }
    }
}