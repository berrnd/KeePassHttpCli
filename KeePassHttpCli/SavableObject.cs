using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace KeePassHttpCli
{
    public abstract class SavableObject
    {
        public static readonly string Folder = String.Format(@"{0}\{1}\", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Assembly.GetExecutingAssembly().GetName().Name);

        public event ObjectSavedEventHandler ObjectSaved;
        public delegate void ObjectSavedEventHandler();
        protected virtual void OnObjectSaved()
        {
            ObjectSavedEventHandler handler = this.ObjectSaved;
            if (handler != null)
                handler();
        }

        public void Save(string Filename)
        {
            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);

            string filePath = Path.Combine(Folder, Filename);

            XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
            using (StringWriter stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, this);
                stringWriter.Flush();
                stringWriter.Close();
                File.WriteAllText(filePath, EncryptString(stringWriter.ToString()), Encoding.Unicode);
            }

            this.OnObjectSaved();
        }

        public static T Load<T>(string Filename) where T : SavableObject, new()
        {
            string filePath = Path.Combine(Folder, Filename);

            if ((File.Exists(filePath)))
            {
                try
                {
                    string decryptedFileContents = DecryptString(File.ReadAllText(filePath, Encoding.Unicode));

                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    T deserializedObject;
                    using (StringReader stringReader = new StringReader(decryptedFileContents))
                    {
                        deserializedObject = ((T)xmlSerializer.Deserialize(stringReader));
                    }

                    return deserializedObject;
                }
                catch (Exception)
                {
                    return new T();
                }
            }
            else
                return new T();
        }

        public static String EncryptString(string text)
        {
            byte[] encryptedData = ProtectedData.Protect(Encoding.Unicode.GetBytes(text), null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        public static String DecryptString(string text)
        {
            byte[] decryptedData = ProtectedData.Unprotect(Convert.FromBase64String(text), null, DataProtectionScope.CurrentUser);
            return Encoding.Unicode.GetString(decryptedData);
        }

    }
}