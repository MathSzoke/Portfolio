export function openSwitchProfile()
{
    document.querySelector(".style-switcher-profile").classList.toggle("open");
}

// fechar janela de cores dos destaques
export function closeSwitchProfile()
{
    if(document.querySelector(".style-switcher-profile").classList.contains("open"))
    {
        document.querySelector(".style-switcher-profile").classList.remove("open");
    }
}
