namespace FakeItEasy.SelfInitializedFakes
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;

    internal class FileStorage
        : ICallStorage
    {
        private readonly string fileName;
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStorage"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileSystem">The file system.</param>
        public FileStorage(string fileName, IFileSystem fileSystem)
        {
            this.fileName = fileName;
            this.fileSystem = fileSystem;
        }

        /// <summary>
        /// A factory responsible for creating instances of FileStorage.
        /// </summary>
        /// <param name="fileName">The file name of the storage.</param>
        /// <returns>A FileStorage instance.</returns>
        public delegate FileStorage Factory(string fileName);

        private bool FileExists
        {
            get { return this.fileSystem.FileExists(this.fileName); }
        }

        /// <summary>
        /// Loads the recorded calls for the specified recording.
        /// </summary>
        /// <returns>
        /// The recorded calls for the recording with the specified id.
        /// </returns>
        public IEnumerable<CallData> Load()
        {
            if (!this.FileExists)
            {
                return null;
            }

            return this.LoadCallsFromFile();
        }

        /// <summary>
        /// Saves the specified calls as the recording with the specified id,
        /// overwriting any previous recording.
        /// </summary>
        /// <param name="calls">The calls to save.</param>
        public void Save(IEnumerable<CallData> calls)
        {
            this.EnsureThatFileExists();
            this.WriteCallsToFile(calls);
        }

        private void WriteCallsToFile(IEnumerable<CallData> calls)
        {
#if FEATURE_SERIALIZATION
            using (var file = this.fileSystem.Open(this.fileName, FileMode.Truncate))
            using (var sw = new StreamWriter(file))
            using (var jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = Formatting.None;
                var serializer = new JsonSerializer();
                serializer.TypeNameHandling = TypeNameHandling.All;
                serializer.Serialize(jsonWriter, calls.ToList());
                jsonWriter.Flush();
            }
#else
            throw new System.NotImplementedException();
#endif
        }

        private IEnumerable<CallData> LoadCallsFromFile()
        {
#if FEATURE_SERIALIZATION
            using (var file = this.fileSystem.Open(this.fileName, FileMode.Open))
            using (var sr = new StreamReader(file))
            using (var jsonReader = new JsonTextReader(sr))
            {
                var text = sr.ReadToEnd();
                var test = JsonConvert.DeserializeObject<List<CallData>>(text);
                var deserializer = new JsonSerializer();
                deserializer.TypeNameHandling = TypeNameHandling.All;
                var result = deserializer.Deserialize<List<CallData>>(jsonReader);
                return result;
            }
#else
            throw new System.NotImplementedException();
#endif
        }

        private void EnsureThatFileExists()
        {
            if (!this.FileExists)
            {
                this.fileSystem.Create(this.fileName);
            }
        }
    }
}
