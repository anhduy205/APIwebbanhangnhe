const apiUrlInput = document.getElementById("apiUrl");
const productForm = document.getElementById("productForm");
const productIdInput = document.getElementById("productId");
const productNameInput = document.getElementById("productName");
const priceInput = document.getElementById("price");
const descriptionInput = document.getElementById("description");
const productTableBody = document.getElementById("productTableBody");
const detailCard = document.getElementById("detailCard");
const messageBar = document.getElementById("messageBar");
const statusBadge = document.getElementById("statusBadge");
const formTitle = document.getElementById("formTitle");
const btnReset = document.getElementById("btnReset");
const btnRefresh = document.getElementById("btnRefresh");
const btnConnect = document.getElementById("btnConnect");

const storageKey = "product-crud-demo-api-url";

document.addEventListener("DOMContentLoaded", () => {
    loadSavedApiUrl();
    wireEvents();
    fetchProducts();
});

function wireEvents() {
    btnConnect.addEventListener("click", fetchProducts);
    btnRefresh.addEventListener("click", fetchProducts);
    btnReset.addEventListener("click", resetForm);
    productForm.addEventListener("submit", saveProduct);
    productTableBody.addEventListener("click", handleTableClick);
}

function loadSavedApiUrl() {
    const savedUrl = localStorage.getItem(storageKey);
    if (savedUrl) {
        apiUrlInput.value = savedUrl;
    } else if (window.location.origin.startsWith("http")) {
        apiUrlInput.value = `${window.location.origin}/api/products`;
    }
}

function getApiUrl() {
    const value = apiUrlInput.value.trim().replace(/\/+$/, "");
    localStorage.setItem(storageKey, value);
    return value;
}

function buildEndpoint(id) {
    const apiUrl = getApiUrl();
    return id ? `${apiUrl}/${id}` : apiUrl;
}

async function handleResponse(response) {
    if (!response.ok) {
        const message = await response.text();
        throw new Error(message || `Request failed with status ${response.status}`);
    }

    if (response.status === 204) {
        return null;
    }

    const contentType = response.headers.get("content-type") || "";
    if (contentType.includes("application/json")) {
        return response.json();
    }

    return response.text();
}

async function fetchProducts() {
    try {
        setStatus("Loading...", "idle");
        setMessage("Loading product list...");

        const response = await fetch(buildEndpoint());
        const products = await handleResponse(response);

        renderProducts(products);
        setStatus("Connected", "success");
        setMessage(`Loaded ${products.length} product(s).`);
    } catch (error) {
        renderProducts([]);
        setStatus("Connection failed", "error");
        setMessage(error.message);
    }
}

function renderProducts(products) {
    if (!products || products.length === 0) {
        productTableBody.innerHTML = `
            <tr class="table-empty">
                <td colspan="5">No products found. Add a product to begin.</td>
            </tr>
        `;
        return;
    }

    productTableBody.innerHTML = products.map(createRow).join("");
}

function createRow(product) {
    return `
        <tr>
            <td>${product.id}</td>
            <td>${escapeHtml(product.name)}</td>
            <td>${formatPrice(product.price)}</td>
            <td>${escapeHtml(product.description || "")}</td>
            <td class="actions-cell">
                <button class="table-btn" type="button" data-action="view" data-id="${product.id}">View</button>
                <button class="table-btn" type="button" data-action="edit" data-id="${product.id}">Edit</button>
                <button class="table-btn delete" type="button" data-action="delete" data-id="${product.id}">Delete</button>
            </td>
        </tr>
    `;
}

async function handleTableClick(event) {
    const actionButton = event.target.closest("[data-action]");
    if (!actionButton) {
        return;
    }

    const id = Number(actionButton.dataset.id);
    const action = actionButton.dataset.action;

    if (action === "view") {
        await viewProduct(id);
        return;
    }

    if (action === "edit") {
        await editProduct(id);
        return;
    }

    if (action === "delete") {
        await deleteProduct(id);
    }
}

async function viewProduct(id) {
    try {
        setMessage(`Loading product #${id}...`);
        const response = await fetch(buildEndpoint(id));
        const product = await handleResponse(response);
        renderDetail(product);
        setMessage(`Showing product #${id}.`);
    } catch (error) {
        setMessage(error.message);
    }
}

async function editProduct(id) {
    try {
        const response = await fetch(buildEndpoint(id));
        const product = await handleResponse(response);

        productIdInput.value = product.id;
        productNameInput.value = product.name;
        priceInput.value = product.price;
        descriptionInput.value = product.description || "";
        formTitle.textContent = `Edit product #${product.id}`;
        renderDetail(product);
        setMessage(`Editing product #${product.id}.`);
        productNameInput.focus();
    } catch (error) {
        setMessage(error.message);
    }
}

async function saveProduct(event) {
    event.preventDefault();

    const product = {
        id: productIdInput.value ? Number(productIdInput.value) : 0,
        name: productNameInput.value.trim(),
        price: Number(priceInput.value),
        description: descriptionInput.value.trim()
    };

    if (!product.name || Number.isNaN(product.price) || product.price <= 0) {
        setMessage("Please enter a product name and a valid price.");
        return;
    }

    const isEditMode = product.id > 0;
    const requestUrl = isEditMode ? buildEndpoint(product.id) : buildEndpoint();
    const requestMethod = isEditMode ? "PUT" : "POST";

    try {
        setMessage(isEditMode ? `Updating product #${product.id}...` : "Creating product...");

        const response = await fetch(requestUrl, {
            method: requestMethod,
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(product)
        });

        const savedProduct = await handleResponse(response);
        const productToShow = savedProduct || product;

        resetForm();
        await fetchProducts();
        renderDetail(productToShow);
        setMessage(isEditMode ? "Product updated successfully." : "Product created successfully.");
    } catch (error) {
        setMessage(error.message);
    }
}

async function deleteProduct(id) {
    const confirmed = window.confirm(`Delete product #${id}?`);
    if (!confirmed) {
        return;
    }

    try {
        setMessage(`Deleting product #${id}...`);
        const response = await fetch(buildEndpoint(id), { method: "DELETE" });
        await handleResponse(response);

        if (Number(productIdInput.value) === id) {
            resetForm();
        }

        renderEmptyDetail();
        await fetchProducts();
        setMessage(`Deleted product #${id}.`);
    } catch (error) {
        setMessage(error.message);
    }
}

function resetForm() {
    productForm.reset();
    productIdInput.value = "";
    formTitle.textContent = "Add product";
}

function renderDetail(product) {
    detailCard.classList.remove("empty");
    detailCard.innerHTML = `
        <dl class="detail-meta">
            <div>
                <dt>Product ID</dt>
                <dd>${product.id}</dd>
            </div>
            <div>
                <dt>Name</dt>
                <dd>${escapeHtml(product.name)}</dd>
            </div>
            <div>
                <dt>Price</dt>
                <dd>${formatPrice(product.price)}</dd>
            </div>
            <div>
                <dt>Description</dt>
                <dd>${escapeHtml(product.description || "")}</dd>
            </div>
        </dl>
    `;
}

function renderEmptyDetail() {
    detailCard.classList.add("empty");
    detailCard.innerHTML = "<p>Select a product from the list to view details.</p>";
}

function setMessage(message) {
    messageBar.textContent = message;
}

function setStatus(text, state) {
    statusBadge.textContent = text;
    statusBadge.className = `status-badge ${state}`;
}

function formatPrice(value) {
    const amount = Number(value);
    return `${amount.toLocaleString("en-US")} VND`;
}

function escapeHtml(value) {
    return String(value)
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;")
        .replaceAll("\"", "&quot;")
        .replaceAll("'", "&#39;");
}
