using System;
using Substrate.NetApi.Model.Meta;
using Substrate.NetApi.Model.Types.Metadata.V14;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Globalization;

namespace PlutoWallet.Types
{
    public partial class Metadata
    {
        [JsonProperty("Origin")]
        public string Origin { get; set; }

        [JsonProperty("Magic")]
        public string Magic { get; set; }

        [JsonProperty("Version")]
        public long Version { get; set; }

        [JsonProperty("NodeMetadata")]
        public NodeMetadata NodeMetadata { get; set; }
    }

    public partial class NodeMetadata
    {
        [JsonProperty("Types")]
        public Dictionary<string, TypeValue> Types { get; set; }

        [JsonProperty("Modules")]
        public Dictionary<string, Module> Modules { get; set; }

        [JsonProperty("Extrinsic")]
        public Extrinsic Extrinsic { get; set; }

        [JsonProperty("TypeId")]
        public long TypeId { get; set; }
    }

    public partial class Extrinsic
    {
        [JsonProperty("TypeId")]
        public long TypeId { get; set; }

        [JsonProperty("Version")]
        public long Version { get; set; }

        [JsonProperty("SignedExtensions")]
        public SignedExtension[] SignedExtensions { get; set; }
    }

    public partial class SignedExtension
    {
        [JsonProperty("SignedIdentifier")]
        public string SignedIdentifier { get; set; }

        [JsonProperty("SignedExtType")]
        public long SignedExtType { get; set; }

        [JsonProperty("AddSignedExtType")]
        public long AddSignedExtType { get; set; }
    }

    public partial class Module
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Storage")]
        public Storage Storage { get; set; }

        [JsonProperty("Calls")]
        public Calls Calls { get; set; }

        [JsonProperty("Events")]
        public Calls Events { get; set; }

        [JsonProperty("Constants")]
        public Constant[] Constants { get; set; }

        [JsonProperty("Errors")]
        public Calls Errors { get; set; }

        [JsonProperty("Index")]
        public long Index { get; set; }
    }

    public partial class Calls
    {
        [JsonProperty("TypeId")]
        public long TypeId { get; set; }
    }

    public partial class Constant
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("TypeId")]
        public long TypeId { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }

        [JsonProperty("Docs")]
        public object[] Docs { get; set; }
    }

    public partial class Storage
    {
        [JsonProperty("Prefix")]
        public string Prefix { get; set; }

        [JsonProperty("Entries")]
        public Entry[] Entries { get; set; }
    }

    public partial class Entry
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Modifier")]
        public Modifier Modifier { get; set; }

        [JsonProperty("StorageType")]
        public StorageType StorageType { get; set; }

        [JsonProperty("TypeMap")]
        public TypeMap TypeMap { get; set; }

        [JsonProperty("Default")]
        public string Default { get; set; }

        [JsonProperty("Docs")]
        public object[] Docs { get; set; }
    }

    public partial class TypeMap
    {
        [JsonProperty("Item1")]
        public long Item1 { get; set; }

        [JsonProperty("Item2")]
        public Item2 Item2 { get; set; }
    }

    public partial class Item2
    {
        [JsonProperty("Hashers")]
        public Hasher[] Hashers { get; set; }

        [JsonProperty("Key")]
        public long Key { get; set; }

        [JsonProperty("Value")]
        public long Value { get; set; }
    }

    public partial class TypeValue
    {
        [JsonProperty("TypeFields", NullValueHandling = NullValueHandling.Ignore)]
        public TypeField[] TypeFields { get; set; }

        [JsonProperty("Path", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Path { get; set; }

        [JsonProperty("TypeDef")]
        public TypeDef TypeDef { get; set; }

        [JsonProperty("TypeParams", NullValueHandling = NullValueHandling.Ignore)]
        public TypeParam[] TypeParams { get; set; }

        [JsonProperty("Variants")]
        public Variant[]? Variants { get; set; }

        [JsonProperty("Length", NullValueHandling = NullValueHandling.Ignore)]
        public long? Length { get; set; }

        [JsonProperty("TypeId", NullValueHandling = NullValueHandling.Ignore)]
        public long? TypeId { get; set; }

        [JsonProperty("Primitive", NullValueHandling = NullValueHandling.Ignore)]
        public string Primitive { get; set; }

        [JsonProperty("TypeIds", NullValueHandling = NullValueHandling.Ignore)]
        public long[] TypeIds { get; set; }
    }

    public partial class TypeField
    {
        [JsonProperty("TypeName", NullValueHandling = NullValueHandling.Ignore)]
        public string TypeName { get; set; }

        [JsonProperty("TypeId")]
        public long? TypeId { get; set; }

        [JsonProperty("Name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public partial class TypeParam
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("TypeId", NullValueHandling = NullValueHandling.Ignore)]
        public long? TypeId { get; set; }
    }

    public partial class Variant
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("TypeFields")]
        public TypeField[] TypeFields { get; set; }

        [JsonProperty("Index")]
        public long Index { get; set; }
    }

    public enum Modifier { Default, Optional };

    public enum StorageType { Map, Plain };

    public enum Hasher { BlakeTwo128Concat, Identity, Twox64Concat, Twox128, BlakeTwo128 };

    public enum TypeDef { Array, Compact, Composite, Primitive, Sequence, Tuple, Variant, BitSequence };

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                ModifierConverter.Singleton,
                StorageTypeConverter.Singleton,
                HasherConverter.Singleton,
                TypeDefConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ModifierConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Modifier) || t == typeof(Modifier?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Default":
                    return Modifier.Default;
                case "Optional":
                    return Modifier.Optional;
            }
            throw new Exception("Cannot unmarshal type Modifier");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Modifier)untypedValue;
            switch (value)
            {
                case Modifier.Default:
                    serializer.Serialize(writer, "Default");
                    return;
                case Modifier.Optional:
                    serializer.Serialize(writer, "Optional");
                    return;
            }
            throw new Exception("Cannot marshal type Modifier");
        }

        public static readonly ModifierConverter Singleton = new ModifierConverter();
    }

    internal class StorageTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(StorageType) || t == typeof(StorageType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Map":
                    return StorageType.Map;
                case "Plain":
                    return StorageType.Plain;
            }
            throw new Exception("Cannot unmarshal type StorageType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (StorageType)untypedValue;
            switch (value)
            {
                case StorageType.Map:
                    serializer.Serialize(writer, "Map");
                    return;
                case StorageType.Plain:
                    serializer.Serialize(writer, "Plain");
                    return;
            }
            throw new Exception("Cannot marshal type StorageType");
        }

        public static readonly StorageTypeConverter Singleton = new StorageTypeConverter();
    }

    internal class HasherConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Hasher) || t == typeof(Hasher?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "BlakeTwo128Concat":
                    return Hasher.BlakeTwo128Concat;
                case "Identity":
                    return Hasher.Identity;
                case "Twox64Concat":
                    return Hasher.Twox64Concat;
                case "Twox128":
                    return Hasher.Twox128;
                case "BlakeTwo128":
                    return Hasher.BlakeTwo128;
            }
            throw new Exception("Cannot unmarshal type Hasher");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Hasher)untypedValue;
            switch (value)
            {
                case Hasher.BlakeTwo128Concat:
                    serializer.Serialize(writer, "BlakeTwo128Concat");
                    return;
                case Hasher.Identity:
                    serializer.Serialize(writer, "Identity");
                    return;
                case Hasher.Twox64Concat:
                    serializer.Serialize(writer, "Twox64Concat");
                    return;
                case Hasher.BlakeTwo128:
                    serializer.Serialize(writer, "BlakeTwo128");
                    return;
            }
            throw new Exception("Cannot marshal type Hasher");
        }

        public static readonly HasherConverter Singleton = new HasherConverter();
    }

    internal class TypeDefConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TypeDef) || t == typeof(TypeDef?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Array":
                    return TypeDef.Array;
                case "Compact":
                    return TypeDef.Compact;
                case "Composite":
                    return TypeDef.Composite;
                case "Primitive":
                    return TypeDef.Primitive;
                case "Sequence":
                    return TypeDef.Sequence;
                case "Tuple":
                    return TypeDef.Tuple;
                case "Variant":
                    return TypeDef.Variant;
                case "BitSequence":
                    return TypeDef.BitSequence;
            }
            throw new Exception("Cannot unmarshal type TypeDef");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TypeDef)untypedValue;
            switch (value)
            {
                case TypeDef.Array:
                    serializer.Serialize(writer, "Array");
                    return;
                case TypeDef.Compact:
                    serializer.Serialize(writer, "Compact");
                    return;
                case TypeDef.Composite:
                    serializer.Serialize(writer, "Composite");
                    return;
                case TypeDef.Primitive:
                    serializer.Serialize(writer, "Primitive");
                    return;
                case TypeDef.Sequence:
                    serializer.Serialize(writer, "Sequence");
                    return;
                case TypeDef.Tuple:
                    serializer.Serialize(writer, "Tuple");
                    return;
                case TypeDef.Variant:
                    serializer.Serialize(writer, "Variant");
                    return;
            }
            throw new Exception("Cannot marshal type TypeDef");
        }

        public static readonly TypeDefConverter Singleton = new TypeDefConverter();
    }
}

