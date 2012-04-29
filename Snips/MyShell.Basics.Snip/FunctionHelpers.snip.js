function foreach(list, action) {
    for (var i = 0; i < list.length; i++)
        action(list[i]);
}

function data(id, value) {
    if (arguments.length < 2)
        return app.data(id);
    else
        return app.data(id, value);
}