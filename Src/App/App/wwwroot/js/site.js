const displayData = () => {
    $.ajax({
        type: "GET",
        url: "/Home/testResult",
        dataType: "json"
    }).done(function (result) {
        console.log(result)
    })
}
