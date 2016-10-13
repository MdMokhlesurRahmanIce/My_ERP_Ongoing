var onSuccessForm = function (result) {
    debugger;
        if (result.url) {
            // if the server returned a JSON object containing an url
            // property we redirect the browser to that url
            //window.location.href = result.url;
            //var path = '@Url.Content("~/Login/UserLogin")';

            window.location.href = result.url;

        }
    }
