import Cookies from 'js-cookie';

export function openSwitch()
{
    document.querySelector(".style-switcher").classList.toggle("open");
}

 // tema de cores
const alternateStyles = document.querySelectorAll(".alternate-style");
export function setActiveStyle(color)
{
    alternateStyles.forEach((style) => {
        if (color === style.getAttribute("title"))
        {
            style.removeAttribute("disabled");
        }
        else
        {
            style.setAttribute("disabled", "true");
        }
    })
}

// fechar janela de cores dos destaques
export function closeSwitch()
{
    if(document.querySelector(".style-switcher").classList.contains("open"))
    {
        document.querySelector(".style-switcher").classList.remove("open");
    }
}

window.addEventListener("scroll", () =>
{
    if(document.querySelector(".style-switcher").classList.contains("open"))
    {
        document.querySelector(".style-switcher").classList.remove("open");
    }
})

// dark mode or light mode
// export function darkLightMode()
// {
//     document.body.classList.toggle("dark");
//     Cookies.set('colorTheme', document.body.classList);
// }
