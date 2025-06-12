const darkModeToggle = document.getElementById('darkModeToggle');

// Pokud je tmavý režim aktivní, přidej třídu při načítání stránky
if (localStorage.getItem("darkMode") === "enabled") {
    document.body.classList.add("dark-mode");
}

darkModeToggle.addEventListener('click', () => {
    document.body.classList.toggle('dark-mode');

    // Uložení stavu tmavého režimu
    if (document.body.classList.contains('dark-mode')) {
        localStorage.setItem("darkMode", "enabled");
    } else {
        localStorage.setItem("darkMode", "disabled");
    }
});
