(function waitForSwaggerUI() {
    const intervalId = setInterval(() => {
        if (window.ui) {

            fetch('/antiforgery/token')
                .then(response => response.json())
                .then(data => {
                    const token = data.requestToken;
                    if (token) {
                        window.ui.getConfigs().requestInterceptor = (request) => {
                            request.headers["X-XSRF-TOKEN"] = token;
                            return request;
                        }
                    }
                })
                .catch(err => console.error('Error fetching antiforgery token:', err));

            clearInterval(intervalId);
        }
    }, 500);
})();