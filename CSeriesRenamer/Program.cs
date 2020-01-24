using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSeriesRenamer
{
    class Program
    {
        private static BlockingCollection<String> seriesQueue = new BlockingCollection<String>(1500);

        private static BlockingCollection<String> seasonQueue = new BlockingCollection<String>(1500);

        private static BlockingCollection<String> episodeQueue = new BlockingCollection<String>(1500);

        static void Main(string[] args)
        {
            Console.WriteLine("Beliebige Taste Drücken um zu starten...");
            Console.ReadLine();
            seriesQueue = CreateSeriesQueue(Directory.GetDirectories(Directory.GetCurrentDirectory()));
            //Save
            var files = Task.Run(() =>
            {
                foreach (var filename in episodeQueue.GetConsumingEnumerable())
                {
                    //Rename
                    if(filename.EndsWith(".avi") || filename.EndsWith(".mkv") || filename.EndsWith(".mov"))
                    {
                        String[] SplitteredString = filename.Split('\\');

                        String seriesName = SplitteredString[SplitteredString.Length - 3];

                        String[] seasonArray = SplitteredString[SplitteredString.Length - 2].Split(' ');
                        String seasonNumber = seasonArray[seasonArray.Length - 1];

                        String[] episodeArray = SplitteredString[SplitteredString.Length - 1].Split(' ');
                        String episodeNumber = episodeArray[0];

                        String episodeTitle = String.Empty;

                        for(int i = 1; i < episodeArray.Length; i++)
                        {
                            episodeTitle += String.Format(" {0}", episodeArray[i]);
                        }

                        String newFilename = String.Format("{0} - S{1}E{2} -{3}", seriesName, seasonNumber, episodeNumber, episodeTitle);
                        String newFileString = String.Format("{0}\\Staffel {1}\\{2}", seriesName, seasonNumber, newFilename);

                        File.Move(filename, newFileString);

                        Console.WriteLine(newFilename);
                    }
                }
            });

            //GetSeasons
            var seasons = Task.Run(() =>
            {
                foreach(var seriesFolder in seriesQueue.GetConsumingEnumerable())
                {
                    WriteToSeasonQueue(seriesFolder);
                }
            });

            //GetEpisodes
            var episodes = Task.Run(() =>
            {
                foreach(var seasonFolder in seasonQueue.GetConsumingEnumerable())
                {
                    WriteToEpisodeQueue(seasonFolder);
                }
            });

            Task.WaitAll(files);

            Console.WriteLine("Finished!");
            Console.ReadLine();

        }

        private static BlockingCollection<String> CreateSeriesQueue(string[] seriesFolders)
        {
            var inputQueue = new BlockingCollection<String>();

            foreach (var seriesFolder in seriesFolders)
            {
                inputQueue.Add(seriesFolder);
            }

            inputQueue.CompleteAdding();

            return inputQueue;
        }

        private static void WriteToSeasonQueue(string seriesFolder)
        {
            String[] seasonFolders = Directory.GetDirectories(seriesFolder);

            foreach(var seasonFolder in seasonFolders)
            {
                seasonQueue.Add(seasonFolder);
            }
        }

        private static void WriteToEpisodeQueue(string seasonFolder)
        {
            String[] episodes = Directory.GetFiles(seasonFolder);

            foreach (var episode in episodes)
            {
                episodeQueue.Add(episode);
            }
        }

    }
}