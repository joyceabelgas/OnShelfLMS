function loadNotificationPageData() {
    $.ajax({
        url: '/Notification/GetUserNotifications',
        type: 'GET',
        success: function (response) {
            const list = $('#notificationList');
            list.empty();

            if (response.length === 0) {
                list.append('<li class="text-muted px-2">No notifications found.</li>');
            } else {
                response.forEach(item => {
                    const url = `/${item.method}/${item.action}`;
                    const message = item.message || `${item.method}/${item.action}`;
                    const date = new Date(item.date).toLocaleString('en-US', {
                        month: '2-digit',
                        day: '2-digit',
                        year: '2-digit',
                        hour: '2-digit',
                        minute: '2-digit',
                        hour12: true
                    });

                    list.append(`
                        <li class="notification-item p-3 mb-2">
                            <a href="#" onclick="redirectToNotification(${item.id}, '${url}')" class="text-decoration-none text-dark d-flex justify-content-between align-items-start">
                                <div class="pe-3">
                                    <div class="fw-bold">${item.action}</div>
                                    <div class="small text-muted">– ${message}</div>
                                </div>
                                <div class="text-nowrap text-end small fw-bold text-secondary">${date}</div>
                            </a>
                        </li>
                    `);
                });
            }
        },
        error: function () {
            console.error("Failed to load notifications.");
        }
    });
}

function redirectToNotification(id, url) {
    $.ajax({
        url: '/Notification/MarkAsRead',
        type: 'POST',
        data: { id: id },
        success: function (res) {
            if (res.success) {
                window.location.href = url;
            }
        },
        error: function () {
            window.location.href = url; // fallback redirect
        }
    });
}

$(document).ready(function () {
    loadNotificationPageData();
    setInterval(loadNotificationPageData, 1000);
});