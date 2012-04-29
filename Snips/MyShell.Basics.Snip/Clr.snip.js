function clrload(assemblyPath) {
    return app.clrload(assemblyPath);
}

function clrtype(typename) {
    return app.clrtype(typename);
}

function clrthiscall(obj, method) {
    var argArray = new Array();

    for (var i = 0; i < arguments.length - 2; i++)
        argArray[i] = arguments[i + 2];

    return app.clrcall(0, obj, method, argArray);
}

function clrstaticcall(type, method) {
    var argArray = new Array();

    for (var i = 0; i < arguments.length - 2; i++)
        argArray[i] = arguments[i + 2];

    return app.clrcall(1, type, method, argArray);
}

function clrtypeof(obj) {
    return clrthiscall(obj, "GetType");
}