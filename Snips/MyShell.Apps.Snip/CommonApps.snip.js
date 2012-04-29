function run_aggregate_arguments(args, startIndex, fn) {
    var result = "";
    for (var i = startIndex; i < args.length; i++)
        result += fn(args[i]);

    return result;
}

function run(exeName) {
    return run_internal(exeName + "|?|" + run_aggregate_arguments(arguments, 1, function (arg) { return "|?|a:" + arg; }));
}

function notepad(file) {
    if (file == undefined)
        run("notepad");
    else
        run("notepad", file);
}