using System;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Bson;
using NUnit.Framework;

namespace Paralect.Transitions.Mongo.Test.Tests
{
    [TestFixture]
    public class RenameTest
    {
        [Test]
        public void Test()
        {
            var doc = new BsonDocument()
            {
                { "property1", "value"},
                { "property2", "value23"},
                { "obj", new BsonDocument() {
                    {"inner", "val"},
                    {"inner2", "val"}
                }},

                { "array" , new BsonArray() {
                    new BsonDocument { { "author", "jim" }, { "comment", "I disagree" } },
                    new BsonDocument { { "author", "nancy" }, { "comment", "Good post" } }
                }}
            };

            DeepRename(doc, new string[] {"array" }, "prop343434");
//            DeepRename(doc, new string[] {"array", "author" }, "prop343434");
            DeepRename(doc, new string[] {"property1" }, "prop343434");
            DeepRename(doc, new string[] {"obj", "inner2" }, "prop343434");

            var item = doc["array"].AsBsonArray[0];


        }

        public void DeepRename(BsonValue doc, string[] path, string finalName)
        {
            if (doc.IsBsonArray)
            {
                var array = doc.AsBsonArray;
                foreach (var value in array)
                {
                    DeepRename(value, path, finalName);
                }
                return;
            }

            if (path.Length == 1)
            {
                Rename(doc.AsBsonDocument, path[0], finalName);
                return;
            }

            string[] newPath = new string[path.Length - 1];
            Array.Copy(path, 1, newPath, 0, path.Length - 1);

            var innerValue = doc.AsBsonDocument[path[0]];
            DeepRename(innerValue, newPath, finalName);
        }

        public void Rename(BsonDocument doc, string from, string to)
        {
            doc.Add(to, doc[from]);
            doc.Remove(from);
        }
         
    }
}