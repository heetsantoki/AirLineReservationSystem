function setupSuggest(inputId, boxId) {
    const input = document.getElementById(inputId);
    const box = document.getElementById(boxId);

    input.addEventListener("input", function () {
        let query = this.value.trim();
        if (query.length < 1) { box.innerHTML = ""; return; }

        fetch(`/Flight/GetAirports?term=${encodeURIComponent(query)}`)
            .then(r => r.json())
            .then(data => {
                box.innerHTML = "";
                data.forEach(item => {
                    let div = document.createElement("div");
                    div.classList.add("suggest-item");
                    div.textContent = item;
                    div.onclick = () => { input.value = item; box.innerHTML = ""; };
                    box.appendChild(div);
                });
            });
    });
}

document.addEventListener("DOMContentLoaded", function () {
    setupSuggest("fromInput", "fromSuggest");
    setupSuggest("toInput", "toSuggest");
});
