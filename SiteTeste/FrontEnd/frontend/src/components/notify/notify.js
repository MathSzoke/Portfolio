import './notify.css';

/*
## HeaderType = Message in header notify
## message = message underdown header
## type = type = notify type, can to be any: "error", "info", "success" (this is SUPER sensitive)
*/

let messagesSent = [];

export function notify(headerType, message, type) 
{
    if (messagesSent.indexOf(message) !== -1) 
    {
        // Message already sent before
        // Update expiry time of existing notification
        let notifications = document.getElementById("notification-area").getElementsByClassName("notification");
        for (let i = 0; i < notifications.length; i++) 
        {
            if (notifications[i].innerText === message) 
            {
                // Update notification expiration time
                setTimeout(() => 
                {
                    notifications[i].remove();
                }, 10000);
                return;
            }
        }
    } 
    else 
    {
        // New message
        messagesSent.push(message);
        (()=>{
            let existingDivs = document.querySelectorAll(`.notification.${type}`);
            let hasExistingDiv = false;
            for(let i = 0; i < existingDivs.length; i++)
            {
                if(existingDivs[i].innerText === message)
                {
                    hasExistingDiv = true;
                    break;
                }
            }
            if(!hasExistingDiv)
            {
                let n = document.createElement("div");
                n.addEventListener("click", function(){
                    n.remove();
                });
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

                let txtMessage = document.createElement("div");
                txtMessage.classList.add("txtNotify");
                txtMessage.innerText = message;

                progressBar.appendChild(cdb);
                n.appendChild(header);
                n.appendChild(txtMessage);
                document.getElementById("notification-area").appendChild(n).appendChild(progressBar);

                setTimeout(() => {
                    var notifications = document.getElementById("notification-area").getElementsByClassName("notification");
                    for(let i = 0; i < notifications.length; i++)
                    {
                        if(notifications[i].getAttribute("id") == id)
                        {
                            notifications[i].remove();
                            break;
                        }
                    }
                }, 10000);
            }
        })();
    }
}
