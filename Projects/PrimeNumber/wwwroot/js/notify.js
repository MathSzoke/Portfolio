/*
## HeaderType = Message in header notify
## message = message underdown header
## type = type = notify type, can to be any: "error", "info", "success" (this is SUPER sensitive)
*/

function notify(headerType, message, type)
{
    (() => {
        let n = document.createElement("div");
        let id = Math.random().toString(36);
        n.setAttribute("id", id);
        n.classList.add("notification", type);

        let progressBar = document.createElement("div");
        progressBar.setAttribute("id", id);
        progressBar.classList.add("progressBar");

        let cdb = document.createElement("div"); // cdb = countdown bar
        cdb.setAttribute("id", id);
        cdb.classList.add("cdb", type);

        let header = document.createElement("header");
        header.classList.add("headerNotifications");

        let headerTittle = document.createElement("p");
        header.appendChild(headerTittle);
        headerTittle.classList.add("headerTittle");
        headerTittle.innerText = headerType;

        let fieldClose = document.createElement("div");
        let buttonClose = document.createElement("button");
        buttonClose.setAttribute("id", 'btnCloser');

        header.appendChild(fieldClose);
        fieldClose.classList.add("closeNotification");
        buttonClose.classList.add("closeNotificationBtn");
        buttonClose.innerText = "X";
        fieldClose.appendChild(buttonClose);

        window.addEventListener('load', function () {
            var btnClose = document.getElementById('btnCloser');
            btnClose.addEventListener("click", function () {
                document.body.removeChild(n);
            });
        })

        let txtMessage = document.createElement("div");
        txtMessage.innerText = message;

        progressBar.appendChild(cdb);
        n.appendChild(header);
        n.appendChild(txtMessage);
        document.getElementById("notification-area").appendChild(n).appendChild(progressBar);

        setTimeout(() => {
            var notifications = document.getElementById("notification-area").getElementsByClassName("notification");
            for (let i = 0; i < notifications.length; i++) {
                if (notifications[i].getAttribute("id") == id) {
                    notifications[i].remove();
                    break;
                }
            }
        }, 10000);
    })();
}
