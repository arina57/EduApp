using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace SharedActivities.Core {
    public static class ResourceLoader {

        public static T GetXmlRoot<T>(Assembly assembly, string xmlFile) {
            var serializer = new XmlSerializer(typeof(T));
            var deseializedObject = (T)serializer.Deserialize(GetEmbeddedResourceStream(assembly, xmlFile));
            return deseializedObject;
        }

        /// <summary>
        /// Attempts to find and return the given resource from within the specified assembly.
        /// </summary>
        /// <returns>The embedded resource stream.</returns>
        /// <param name="assembly">Assembly.</param>
        /// <param name="resourceFileName">Resource file name.</param>
        public static Stream GetEmbeddedResourceStream(Assembly assembly, string resourceFileName) {
            var assemblyName = assembly.GetName().Name;
            var resourceNames = assembly.GetManifestResourceNames();
            var fullResourceName = $"{assemblyName}.{resourceFileName}";
            var resourcePaths = resourceNames
                .Where(x => x == fullResourceName)
                .ToArray();

            if (!resourcePaths.Any()) {
                throw new Exception(string.Format("Resource {0} not found.", fullResourceName));
            }

            if (resourcePaths.Count() > 1) {
                throw new Exception($"Multiple resources {fullResourceName} found: {string.Join(" ", resourcePaths)}");
            }

            return assembly.GetManifestResourceStream(resourcePaths.Single());
        }


        /// Attempts to find and return the given resource from within the specified assembly.
        /// </summary>
        /// <returns>The embedded resource as a string.</returns>
        /// <param name="assembly">Assembly.</param>
        /// <param name="resourceFileName">Resource file name.</param>
        public static string GetEmbeddedResourceString(Assembly assembly, string resourceFileName) {
            var stream = GetEmbeddedResourceStream(assembly, resourceFileName);

            using (var streamReader = new StreamReader(stream)) {
                return streamReader.ReadToEnd();
            }
        }
    }
}
