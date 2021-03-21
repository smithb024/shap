namespace Shap.Common.Factories
{
    using System;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// Static factory class, used to read and write to xml files.
    /// </summary>
    public static class XmlFileIo
    {
        /// <summary>
        /// Read from a xml file and serialise into type T.
        /// </summary>
        /// <typeparam name="T">The type which the xml file is exoected to serialise into</typeparam>
        /// <param name="filename">name of the xml file to read</param>
        /// <returns></returns>
        public static T ReadXml<T>(string filename)
        {
            XmlSerializer serialiser = new XmlSerializer(typeof(T));
            T result = default;
            Stream stream = null;

            try
            {
                stream = File.OpenRead(filename);
                result = (T)serialiser.Deserialize(stream);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Directory not found");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Exception {ex}");
                Console.WriteLine($"{ex.InnerException}");
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }

            return result;
        }

        /// <summary>
        /// Export the Type T to an xml file.
        /// </summary>
        /// <typeparam name="T">Type being serialised to a xml file</typeparam>
        /// <param name="obj">object being saved as an xml file</param>
        /// <param name="filename">name of the exported file</param>
        public static void WriteXml<T>(
          T obj,
          string filename)
        {
            var serialiser = new XmlSerializer(typeof(T));

            using (Stream stream = File.Create(filename))
            {
                serialiser.Serialize(
                  stream,
                  obj);
            }
        }
    }
}