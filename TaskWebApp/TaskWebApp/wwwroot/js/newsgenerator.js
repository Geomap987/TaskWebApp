const connection = new signalR.HubConnectionBuilder()
    .withUrl("/newsHub")
    .build();

connection.on("ReceiveNews", function (newsJson) {
    console.log("News received:", newsJson);
    const newsData = JSON.parse(newsJson);
    localStorage.setItem('latestNews', newsJson);
    updateNewsDisplay(newsJson);
});

document.addEventListener("DOMContentLoaded", () => {
    const storedNewsJson = localStorage.getItem('latestNews');
    if (storedNewsJson) {
        updateNewsDisplay(storedNewsJson);
    }
});

connection.start()
    .catch(function (err) {
        console.error(err.toString());
    });

function updateNewsDisplay(newsJson) {
    const newsData = JSON.parse(newsJson);
    const container = document.getElementById('news-container');
    container.innerHTML = '';

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
}

