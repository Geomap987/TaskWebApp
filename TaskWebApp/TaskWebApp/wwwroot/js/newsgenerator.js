const connection = new signalR.HubConnectionBuilder()
    .withUrl("/newsHub")
    .build();

connection.on("ReceiveNews", function (newsJson) {
    console.log("News received:", newsJson);
    const newsData = JSON.parse(newsJson);  // Assuming newsJson is a JSON string
    const container = document.getElementById('news-container');
    console.log("News received:", newsData)
            // Clear previous news links
    ;

            // Assume newsData is an array of news articles
    newsData.data.forEach(article => {
        const description = article.description;
        const url = article.url;

        const newsLink = document.createElement('a');
        newsLink.href = url;
        newsLink.textContent = description ? description : "No description available";
        newsLink.style.textDecoration = 'none';
        newsLink.style.color = 'white';
        newsLink.style.margin = '20px';
        newsLink.style.display = 'block'; 

        container.appendChild(newsLink);
        });
});

connection.start()
    .catch(function (err) {
        console.error(err.toString());
    });

