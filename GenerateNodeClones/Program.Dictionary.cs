namespace GenerateNodeClones;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Contracts;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

internal partial class Program
{
    private static Type[]? AllTypes { get; set; }

    private static void GenerateDictionary(Dictionary<Type, NodeCloneInfo?> nodeTypes, Type seed)
    {
        GenerateAbstractParents(nodeTypes, seed);

        PropertyInfo[] Properties = seed.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        Dictionary<string, NodePropertyInfo> PropertiesInfo = new();
        List<Type> MoreNodeTypes = new();

        foreach (PropertyInfo Property in Properties)
        {
            if (IsPropertyObsolete(Property))
                continue;

            Type PropertyType = Property.PropertyType;

            if (IsSyntaxTokenProperty(PropertyType))
            {
                PropertiesInfo.Add(Property.Name, new NodePropertyInfo() { IsToken = true });
            }
            else
            {
                bool IsNullable = IsPropertyNullable(Property);

                if (IsSyntaxNodeProperty(PropertyType))
                {
                    PropertiesInfo.Add(Property.Name, new NodePropertyInfo() { Node = PropertyType, IsNullable = IsNullable });
                    AddNodeTypeToList(nodeTypes, MoreNodeTypes, PropertyType);
                }

                if (IsSyntaxListProperty(PropertyType, out Type SyntaxListItemType))
                {
                    PropertiesInfo.Add(Property.Name, new NodePropertyInfo() { List = SyntaxListItemType, IsNullable = IsNullable });
                    AddNodeTypeToList(nodeTypes, MoreNodeTypes, SyntaxListItemType);
                }

                if (IsSeparatedSyntaxListProperty(PropertyType, out Type SeparatedSyntaxListItemType))
                {
                    PropertiesInfo.Add(Property.Name, new NodePropertyInfo() { SeparatedList = SeparatedSyntaxListItemType, IsNullable = IsNullable });
                    AddNodeTypeToList(nodeTypes, MoreNodeTypes, SeparatedSyntaxListItemType);
                }
            }
        }

        foreach (Type NodeType in MoreNodeTypes)
            nodeTypes.Add(NodeType, null);

        foreach (Type NodeType in MoreNodeTypes)
            GenerateDictionary(nodeTypes, NodeType);

        nodeTypes[seed] = new NodeCloneInfo(seed, PropertiesInfo);
    }

    private static bool IsSyntaxTokenProperty(Type propertyType)
    {
        return propertyType.IsAssignableTo(typeof(SyntaxToken));
    }

    private static bool IsSyntaxNodeProperty(Type propertyType)
    {
        return propertyType.IsAssignableTo(typeof(SyntaxNode)) && propertyType != typeof(CSharpSyntaxNode);
    }

    private static bool IsSyntaxListProperty(Type propertyType, out Type syntaxListItemType)
    {
        return IsListProperty<SyntaxList<SyntaxNode>>(propertyType, out syntaxListItemType);
    }

    private static bool IsSeparatedSyntaxListProperty(Type propertyType, out Type separatedSyntaxListItemType)
    {
        return IsListProperty<SeparatedSyntaxList<SyntaxNode>>(propertyType, out separatedSyntaxListItemType);
    }

    private static void AddNodeTypeToList(Dictionary<Type, NodeCloneInfo?> nodeTypes, List<Type> moreNodeTypes, Type type)
    {
        List<Type> TypeAndDescendantTypes = GetTypeWithDescendants(type);

        foreach (Type Type in TypeAndDescendantTypes)
            if (!nodeTypes.ContainsKey(Type) && !moreNodeTypes.Contains(Type))
                moreNodeTypes.Add(Type);
    }

    private static bool IsListProperty<T>(Type propertyType, out Type separatedSyntaxListItemType)
    {
        if (propertyType.IsGenericType)
        {
            Type GenericTypeDefinition = propertyType.GetGenericTypeDefinition();
            if (GenericTypeDefinition == typeof(T).GetGenericTypeDefinition())
            {
                Type[] GenericArguments = propertyType.GetGenericArguments();

                Contract.Assert(GenericArguments.Length > 0);
                separatedSyntaxListItemType = GenericArguments[0];

                return true;
            }
        }

        Contract.Unused(out separatedSyntaxListItemType);
        return false;
    }

    private static List<Type> GetTypeWithDescendants(Type type)
    {
        // 'type' is added eventually because it is assignable to itself.
        List<Type> Result = new();

        Type[] AssemblyTypes = AllTypes ?? type.Assembly.GetTypes();

        foreach (Type AssemblyType in AssemblyTypes)
            if (!AssemblyType.IsAbstract && AssemblyType.IsPublic && AssemblyType.IsAssignableTo(type))
                Result.Add(AssemblyType);

        return Result;
    }

    private static bool IsPropertyNullable(PropertyInfo property)
    {
        return NullabilityInfoContext.Create(property).ReadState == NullabilityState.Nullable;
    }

    private static bool IsPropertyObsolete(PropertyInfo property)
    {
        return property.CustomAttributes.Any(AttributeData => IsObsoleteAttributeType(AttributeData.AttributeType));
    }

    private static bool IsObsoleteAttributeType(Type attributeType)
    {
        return attributeType.Name == nameof(ObsoleteAttribute) && attributeType.Namespace == typeof(Type).Namespace;
    }

    private static void GenerateAbstractParents(Dictionary<Type, NodeCloneInfo?> nodeTypes, Type seed)
    {
        Type SyntaxType = seed;

        while (SyntaxType.BaseType is Type BaseType)
        {
            if (BaseType.Name == nameof(CSharpSyntaxNode))
                break;

            Debug.Assert(BaseType.IsAbstract);
            GenerateAbstractParent(nodeTypes, BaseType);

            SyntaxType = BaseType;
        }
    }

    private static void GenerateAbstractParent(Dictionary<Type, NodeCloneInfo?> nodeTypes, Type baseType)
    {
        if (nodeTypes.ContainsKey(baseType))
            return;

        NodeCloneInfo Parent = new(baseType, new Dictionary<string, NodePropertyInfo>());
        nodeTypes.Add(baseType, Parent);
    }

    private static readonly NullabilityInfoContext NullabilityInfoContext = new();
}
