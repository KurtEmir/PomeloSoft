console.log("PomeloSoft Stock Management App is running!");

document.addEventListener('DOMContentLoaded', () => {
    const apiBaseUrl = 'http://localhost:5008/api';
    const productGrid = document.getElementById('product-grid');
    const categoryFilters = document.getElementById('category-filters');
    const brandFilters = document.getElementById('brand-filters');
    const searchInput = document.getElementById('search-input'); 
    const showInactiveToggle = document.getElementById('show-inactive-toggle'); 
    const notificationArea = document.getElementById('notification-area');

    const addCategoryModal = document.getElementById('add-category-modal');
    const addCategoryBtn = document.getElementById('add-category-btn');
    const closeBtn = addCategoryModal.querySelector('.close-btn');
    const cancelCategoryBtn = document.getElementById('cancel-category-btn');
    const addCategoryForm = document.getElementById('add-category-form');
    const modalNotification = document.getElementById('modal-notification');

    const deactivateCategoryModal = document.getElementById('deactivate-category-modal');
    const deactivateCategoryBtn = document.getElementById('deactivate-category-btn');
    const closeDeactivateBtn = deactivateCategoryModal.querySelector('.close-btn');
    const cancelDeactivateBtn = document.getElementById('cancel-deactivate-btn');
    const deactivateCategoryForm = document.getElementById('deactivate-category-form');
    const deactivateModalNotification = document.getElementById('deactivate-modal-notification');
    const categoryToDeactivateSelect = document.getElementById('category-to-deactivate');


    let allProducts = [];
    let allCategories = [];

    // --- Data Loading Functions ---
    async function loadProducts() {
        const showInactive = showInactiveToggle.checked;
        const endpoint = showInactive ? `${apiBaseUrl}/products/inActive` : `${apiBaseUrl}/products`;
        try {
            const response = await fetch(endpoint);
            if (!response.ok) throw new Error('Network response was not ok for products.');
            const products = await response.json();
            // Sort products by creation date, newest first
            products.sort((a, b) => new Date(b.creationDate) - new Date(a.creationDate));
            allProducts = products;
        } catch (error) {
            console.error('Failed to fetch products:', error);
            productGrid.innerHTML = '<p>An error occurred while loading products.</p>';
        }
    }

    async function loadCategories() {
        try {
            const response = await fetch(`${apiBaseUrl}/categories`);
            if (!response.ok) throw new Error('Could not fetch categories');
            allCategories = await response.json();
            renderCategories();
        } catch (error) {
            console.error('Failed to fetch categories:', error);
            categoryFilters.innerHTML = 'Error loading categories.';
        }
    }

    function renderProducts(products) {
        productGrid.innerHTML = '';
        if (products.length === 0) {
            productGrid.innerHTML = '<p>No products match the current filters.</p>';
            return;
        }
        products.forEach(product => {
            const card = document.createElement('div');
            card.className = `product-card ${!product.isActive ? 'inactive-product' : ''}`;

            const actionButtons = product.isActive
                ? `
                    <button class="card-actions-btn">
                        <i class="fas fa-ellipsis-v"></i>
                    </button>
                    <div class="dropdown-menu">
                        <a href="edit-product.html?id=${product.id}" class="dropdown-item">
                            <i class="fas fa-edit"></i> Edit
                        </a>
                        <a href="#" class="dropdown-item delete-btn" data-id="${product.id}">
                            <i class="fas fa-trash-alt"></i> Delete
                        </a>
                    </div>`
                : `
                    <button class="card-actions-btn restore-btn" data-id="${product.id}" title="Restore Product">
                        <i class="fas fa-undo-alt"></i>
                    </button>`;
            
            card.innerHTML = `
                <div class="card-actions">
                    ${actionButtons}
                </div>
                <img src="${product.imageThumbnailUrl || 'https://via.placeholder.com/250'}" alt="${product.name}" class="product-image">
                <div class="product-brand">${product.brand}</div>
                <h3 class="product-name">${product.name}</h3>
                <div class="product-price">${product.price.toLocaleString('tr-TR', { style: 'currency', currency: 'TRY' })}</div>
                <div class="product-stock" style="color: ${product.stock > 0 ? 'green' : 'red'};">${product.stock > 0 ? `${product.stock} in stock` : 'Out of Stock'}</div>
            `;
            productGrid.appendChild(card);
        });
    }

    function renderCategories() {
        categoryFilters.innerHTML = '';
        allCategories.forEach(category => {
            const filterItem = document.createElement('div');
            filterItem.innerHTML = `<label><input type="checkbox" name="category" value="${category.id}"> ${category.name}</label>`;
            categoryFilters.appendChild(filterItem);
        });
    }

    function renderBrands() {
        brandFilters.innerHTML = '';
        const brands = [...new Set(allProducts.map(p => p.brand))];
        brands.sort().forEach(brand => {
            const filterItem = document.createElement('div');
            filterItem.innerHTML = `<label><input type="checkbox" name="brand" value="${brand}"> ${brand}</label>`;
            brandFilters.appendChild(filterItem);
        });
    }

    // --- Filtering Logic ---
    function applyFilters() {
        const searchTerm = searchInput.value.toLowerCase();
        const selectedCategories = [...categoryFilters.querySelectorAll('input:checked')].map(input => parseInt(input.value));
        const selectedBrands = [...brandFilters.querySelectorAll('input:checked')].map(input => input.value);

        let filteredProducts = allProducts;

        if (searchTerm) {
            filteredProducts = filteredProducts.filter(p => p.name.toLowerCase().includes(searchTerm));
        }
        if (selectedCategories.length > 0) {
            filteredProducts = filteredProducts.filter(p => selectedCategories.includes(p.categoryId));
        }
        if (selectedBrands.length > 0) {
            filteredProducts = filteredProducts.filter(p => selectedBrands.includes(p.brand));
        }

        renderProducts(filteredProducts);
    }

    // --- Event Listeners Setup ---
    function setupEventListeners() {
        showInactiveToggle.addEventListener('change', async () => {
            await loadProducts();
            applyFilters();
        });
        searchInput.addEventListener('input', applyFilters);
        categoryFilters.addEventListener('change', applyFilters);
        brandFilters.addEventListener('change', applyFilters);

        // Modal listeners
        addCategoryBtn.addEventListener('click', openModal);
        closeBtn.addEventListener('click', closeModal);
        cancelCategoryBtn.addEventListener('click', closeModal);

        deactivateCategoryBtn.addEventListener('click', openDeactivateModal);
        closeDeactivateBtn.addEventListener('click', closeDeactivateModal);
        cancelDeactivateBtn.addEventListener('click', closeDeactivateModal);
        
        window.addEventListener('click', (event) => {
            if (event.target == addCategoryModal) {
                closeModal();
            }
            if (event.target == deactivateCategoryModal) {
                closeDeactivateModal();
            }
        });

        // Form submission
        addCategoryForm.addEventListener('submit', handleCategorySubmit);
        deactivateCategoryForm.addEventListener('submit', handleDeactivateCategorySubmit);

        // Event delegation for dynamic content (delete, restore, dropdown)
        document.body.addEventListener('click', handleActionButtons);
    }
    
    async function handleCategorySubmit(e) {
        e.preventDefault();
        const formData = new FormData(addCategoryForm);
        const categoryData = {
            name: formData.get('name')
        };

        try {
            const response = await fetch(`${apiBaseUrl}/categories`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(categoryData)
            });

            if (response.ok) {
                closeModal();
                showNotification('Category added successfully!', 'success');
                await loadCategories(); // Just reload categories
                // No need to call applyFilters() here as no product filters depend on a new category yet
            } else {
                const errorText = await response.text();
                showModalNotification(errorText || 'Failed to add category.');
            }
        } catch (error) {
            console.error('Error adding category:', error);
            showModalNotification('An error occurred while connecting to the server.');
        }
    }

    async function handleDeactivateCategorySubmit(e) {
        e.preventDefault();
        const categoryId = categoryToDeactivateSelect.value;
        if (!categoryId) {
            showDeactivateModalNotification('Please select a category.');
            return;
        }

        try {
            const response = await fetch(`${apiBaseUrl}/categories/${categoryId}`, {
                method: 'DELETE',
            });

            if (response.ok) {
                closeDeactivateModal();
                showNotification('Category deactivated successfully!', 'success');
                await init();
            } else {
                const errorText = await response.text();
                showDeactivateModalNotification(errorText || 'Failed to deactivate category.');
            }
        } catch (error) {
            console.error('Error deactivating category:', error);
            showDeactivateModalNotification('An error occurred while connecting to the server.');
        }
    }

    async function handleActionButtons(e) {
        const isActionsButton = e.target.closest('.card-actions-btn:not(.restore-btn)');
        const restoreButton = e.target.closest('.restore-btn');
        const deleteButton = e.target.closest('.delete-btn');

        if (restoreButton) {
            e.preventDefault();
            const productId = restoreButton.dataset.id;
            if (!confirm(`Are you sure you want to restore product ID ${productId}?`)) return;
            try {
                const response = await fetch(`${apiBaseUrl}/products/${productId}/restore`, { method: 'PUT' });
                if (response.ok) {
                    await loadProducts();
                    applyFilters();
                } else {
                    alert(`Failed to restore product: ${await response.text()}`);
                }
            } catch (error) {
                console.error('Restore error:', error);
                alert('An error occurred while connecting to the server.');
            }
        } else if (deleteButton) {
            e.preventDefault();
            const productId = deleteButton.dataset.id;
            if (!confirm(`Are you sure you want to delete product ID ${productId}?`)) return;
            try {
                const response = await fetch(`${apiBaseUrl}/products/${productId}`, { method: 'DELETE' });
                if (response.ok) {
                    // Visually remove the card instead of full reload for better UX
                    deleteButton.closest('.product-card').remove();
                    // Also remove from the main list to keep state consistent
                    allProducts = allProducts.filter(p => p.id != productId);
                } else {
                    alert('An error occurred while deleting the product.');
                }
            } catch (error) {
                console.error('Delete failed:', error);
                alert('An error occurred while connecting to the server.');
            }
        } else if (isActionsButton) {
            const dropdown = isActionsButton.nextElementSibling;
            document.querySelectorAll('.dropdown-menu.show').forEach(d => {
                if (d !== dropdown) d.classList.remove('show');
            });
            dropdown.classList.toggle('show');
        } else if (!e.target.closest('.card-actions')) {
            document.querySelectorAll('.dropdown-menu.show').forEach(d => d.classList.remove('show'));
        }
    }
    
    // --- Utility Functions ---
    function showNotification(message, type = 'success') {
        notificationArea.textContent = message;
        notificationArea.className = `notification ${type}`;
        notificationArea.style.display = 'block';
        setTimeout(() => {
            notificationArea.style.display = 'none';
        }, 3000);
    }
    
    function showModalNotification(message) {
        modalNotification.textContent = message;
        modalNotification.className = 'notification error';
        modalNotification.style.display = 'block';
    }

    function showDeactivateModalNotification(message) {
        deactivateModalNotification.textContent = message;
        deactivateModalNotification.className = 'notification error';
        deactivateModalNotification.style.display = 'block';
    }
    
    function openModal() {
        addCategoryModal.style.display = 'block';
        modalNotification.style.display = 'none';
    }

    function closeModal() {
        addCategoryModal.style.display = 'none';
        addCategoryForm.reset();
    }

    async function openDeactivateModal() {
        // Populate the select dropdown with current active categories
        categoryToDeactivateSelect.innerHTML = '<option value="">Select a category...</option>';
        allCategories.filter(c => c.isActive).forEach(cat => {
            const option = document.createElement('option');
            option.value = cat.id;
            option.textContent = cat.name;
            categoryToDeactivateSelect.appendChild(option);
        });
        deactivateModalNotification.style.display = 'none';
        deactivateCategoryModal.style.display = 'block';
    }

    function closeDeactivateModal() {
        deactivateCategoryModal.style.display = 'none';
        deactivateCategoryForm.reset();
    }

    // --- Initial Load ---
    async function init() {
        await Promise.all([loadProducts(), loadCategories()]);
        renderBrands();
        applyFilters();
        setupEventListeners();
        checkSuccessMessage();
    }
    
    function checkSuccessMessage() {
        const msg = sessionStorage.getItem('successMessage');
        if (msg) {
            showNotification(msg, 'success');
            sessionStorage.removeItem('successMessage');
        }
    }

    init();
});
