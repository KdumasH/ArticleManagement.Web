function mostrarToast(mensaje, tipo) {
    const bootstrapType = `text-${tipo}`;

    // Contenedor del toast
    const toastContainer = document.createElement("div");
    toastContainer.className = `toast align-items-center ${bootstrapType} border-0`;
    toastContainer.role = "alert";
    toastContainer.innerHTML = `
        <div class="d-flex">
            <div class="toast-body">${mensaje}</div>
            <button type="button" class="btn-close btn-close-white me-2 m-auto"
                    data-bs-dismiss="toast" aria-label="Close"></button>
        </div>
    `;

    // Posicionar en esquina superior derecha
    const wrapper = document.createElement("div");
    wrapper.className = "toast-container position-fixed top-0 end-0 p-3";
    wrapper.style.zIndex = "9999";
    wrapper.appendChild(toastContainer);
    document.body.appendChild(wrapper);

    // Mostrar toast
    new bootstrap.Toast(toastContainer, { delay: 5000 }).show();
}
