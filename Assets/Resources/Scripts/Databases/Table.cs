using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Cosmos {
    #region Field
    public class Field {

        public string record;
        public string name;
        public object value;
        public Field(string Name, object Value, string Record = "") {
            name = Name;
            value = Value;
            record = Record;
        }

    }
    #endregion
    #region Table
    public class Table {
        #region variables 
        public string name;
        public int count;
        Dictionary<string, Dictionary<string, Field>> Records = new Dictionary<string, Dictionary<string, Field>>();
        #endregion
        public Table(string Name) {
            name = Name;
        }


        public List<String> GetKeys() {
            return new List<String>(Records.Keys);
        }
        public List<String> GetFieldKeys() {
            List<String> keys = new List<String>();
            foreach (Dictionary<string, Field> rec in Records.Values) {
                foreach (string col in rec.Keys) {
                    if (!keys.Contains(col)) {
                        keys.Add(col);
                    }
                }
            }
            return keys;
        }
        #region Records

        public Dictionary<string, Field> AddRecord(string Id) {
            Dictionary<string, Field> fields = new Dictionary<string, Field>();
            Records.Add(Id, fields);
            count++;
            return fields;
        }
        public List<Dictionary<string, Field>> GetAllRecords(string Category) {
            List<Dictionary<string, Field>> recs = new List<Dictionary<string, Field>>();
            foreach (KeyValuePair<string, Dictionary<string, Field>> rec in Records) {
                if (rec.Key == Category) {
                    recs.Add(rec.Value);
                } else {
                    foreach (KeyValuePair<string, Field> field in rec.Value) {
                        if (field.Key == Category) {
                            recs.Add(rec.Value);
                        }
                    }
                }
            }
            return recs;
        }
        #endregion

        #region Fields
        public Field UpdateField(string ID, Field field) {
            Dictionary<string, Field> fields;
            //Check Record      
            Records.TryGetValue(ID, out fields);
            if (fields == null) {
                fields = AddRecord(ID);
            }

            //Check Field
            Field tField;
            fields.TryGetValue(field.name, out tField);
            if (tField == null) {
                fields.Add(field.name, field);
            }
            tField = field;
            tField.record = ID;
            return tField;
        }
        public Field UpdateField(string ID, string name, object value) {
            return UpdateField(ID, new Field(name, value));
        }
        #endregion

        #region Access Methods

        public object GetValue(string ID, string FieldName) {
            Dictionary<string, Field> record = null;
            Records.TryGetValue(ID, out record);
            if (record == null) return null;
            Field field = null;
            record.TryGetValue(FieldName, out field);
            if (field == null) return null;
            return field.value;
        }

        #endregion



        public void Print() {
            Debugger.Log("Printing table: " + name + " ---");
            List<Dictionary<string, Field>> recs = new List<Dictionary<string, Field>>();
            foreach (KeyValuePair<string, Dictionary<string, Field>> r in Records) {
                recs.Add(r.Value);
            }
            foreach (Dictionary<string, Field> dic in recs) {
                foreach (Field f in dic.Values) {
                    Debugger.Log("Record: " + f.record + " | Field: " + f.name + " = " + f.value.ToString());
                }
            }
            Debugger.Log("--- Print end: " + name);
        }
    }
    #endregion
}