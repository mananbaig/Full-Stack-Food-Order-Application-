// Connect to SignalR notification hub and update DOM
(function () {
    const script = document.createElement('script');
    script.src = 'https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/7.0.0/signalr.min.js';
    script.onload = () => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl('/hubs/notifications')
            .withAutomaticReconnect()
            .build();

        connection.on('ReceiveNotification', function (message) {
            const container = document.getElementById('notifications');
            if (container) {
                const li = document.createElement('li');
                li.className = 'small';
                li.textContent = message;
                if (container.firstElementChild && container.firstElementChild.tagName === 'UL') {
                    container.firstElementChild.insertBefore(li, container.firstElementChild.firstChild);
                } else {
                    const ul = document.createElement('ul');
                    ul.className = 'list-unstyled mb-0';
                    ul.appendChild(li);
                    container.appendChild(ul);
                }
            }
        });

        connection.on('CartUpdated', function (count) {
            // Replace the badge number by refreshing the CartBadge via fetch partial might be better,
            // but here we try a quick DOM update
            const badges = document.querySelectorAll('.fa-shopping-cart + .badge, .fa-shopping-cart ~ .badge');
            badges.forEach(b => b.textContent = count);
        });

        connection.start().catch(err => console.error(err.toString()));
    };
    document.head.appendChild(script);
})();
