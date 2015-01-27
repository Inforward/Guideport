using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel.Description;

namespace Portal.Infrastructure.ServiceModel
{
    class DataContractByRefSerializerOperationBehavior : DataContractSerializerOperationBehavior
    {
        public DataContractByRefSerializerOperationBehavior(OperationDescription operation) : base(operation) { }

        public override XmlObjectSerializer CreateSerializer(Type type, string name, string ns, IList<Type> knownTypes)
        {
            return new DataContractSerializer(type, name, ns, knownTypes, MaxItemsInObjectGraph, IgnoreExtensionDataObject, true, DataContractSurrogate);
        }

        public override XmlObjectSerializer CreateSerializer(Type type, System.Xml.XmlDictionaryString name, System.Xml.XmlDictionaryString ns, IList<Type> knownTypes)
        {
            return new DataContractSerializer(type, name, ns, knownTypes, MaxItemsInObjectGraph, IgnoreExtensionDataObject, true, DataContractSurrogate);
        }
    }
}