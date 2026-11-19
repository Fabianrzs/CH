using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace MockIntegrations.Tests.Infrastructure;

/// <summary>Crea los proxies WCF de GA2 apuntando al mock con un binding equivalente al de GA2.</summary>
public static class SoapClients
{
    /// <summary>
    /// <c>BasicHttpBinding</c> sin seguridad (GA2 usa <c>None</c> con URL http), mensajes grandes y un
    /// timeout acotado para que una prueba no quede colgada (GA2 usa <c>TimeSpan.MaxValue</c>).
    /// </summary>
    public static Binding Binding() => new BasicHttpBinding(BasicHttpSecurityMode.None)
    {
        MaxReceivedMessageSize = int.MaxValue,
        MaxBufferSize = int.MaxValue,
        SendTimeout = TimeSpan.FromSeconds(30),
        ReceiveTimeout = TimeSpan.FromSeconds(30),
    };

    public static TClient Create<TClient>(MockServer server, string path) where TClient : class =>
        (TClient)Activator.CreateInstance(typeof(TClient), Binding(), new EndpointAddress(new Uri(server.BaseUri, path.TrimStart('/'))))!;

    /// <summary>
    /// Invoca <c>{operation}Async</c> en un proxy con un request vacío (como los ejemplos de las fichas)
    /// y devuelve el response. Sirve para recorrer todas las operaciones de un contrato.
    /// </summary>
    public static async Task<object?> InvokeEmptyAsync(object client, string operation)
    {
        var method = client.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m.Name == operation + "Async")
            .OrderBy(m => m.GetParameters().Length)
            .First();
        var args = method.GetParameters().Select(p => Default(p.ParameterType)).ToArray();
        var task = (Task)method.Invoke(client, args)!;
        await task;
        return task.GetType().GetProperty("Result")!.GetValue(task);
    }

    /// <summary>Extrae <c>{operation}Result</c> de un response (directo o dentro de <c>Body</c>).</summary>
    public static object? Result(object? response, string operation)
    {
        if (response is null)
        {
            return null;
        }

        var name = operation + "Result";
        if (Member(response, name, out var direct))
        {
            return direct;
        }

        return Member(response, "Body", out var body) && body is not null && Member(body, name, out var nested) ? nested : null;

        static bool Member(object target, string member, out object? value)
        {
            var t = target.GetType();
            if (t.GetField(member) is { } f)
            {
                value = f.GetValue(target);
                return true;
            }

            if (t.GetProperty(member) is { } p)
            {
                value = p.GetValue(target);
                return true;
            }

            value = null;
            return false;
        }
    }

    private static object? Default(Type type)
    {
        if (type == typeof(string))
        {
            return "1";
        }

        if (type.IsValueType)
        {
            return Activator.CreateInstance(type);
        }

        if (type.IsArray || type.GetConstructor(Type.EmptyTypes) is null)
        {
            return null;
        }

        var instance = Activator.CreateInstance(type)!;
        // Mensajes "wrapped" (XxxRequest { Body = XxxRequestBody }): se instancia el cuerpo.
        foreach (var field in type.GetFields().Where(f => f.GetValue(instance) is null && !f.FieldType.IsArray && f.FieldType != typeof(string) && f.FieldType.GetConstructor(Type.EmptyTypes) is not null))
        {
            field.SetValue(instance, Activator.CreateInstance(field.FieldType));
        }

        return instance;
    }
}
