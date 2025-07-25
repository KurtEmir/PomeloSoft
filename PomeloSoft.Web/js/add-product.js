document.addEventListener('DOMContentLoaded', () => {
    const apiBaseUrl = 'http://localhost:5008/api'; 
    const form = document.getElementById('add-product-form');
    const categorySelect = document.getElementById('category');
    const notificationArea = document.getElementById('form-notification');

    function clearErrors() {
        document.querySelectorAll('.error-message').forEach(el => el.textContent = '');
        notificationArea.style.display = 'none';
        notificationArea.textContent = '';
        notificationArea.className = 'notification';
    }

    function displayErrors(errors) {
        for (const key in errors) {
            // Field-specific errors (e.g., from Data Annotations)
            const inputName = key.charAt(0).toLowerCase() + key.slice(1);
            const errorDiv = form.querySelector(`[name="${inputName}"] + .error-message`);
            if (errorDiv) {
                errorDiv.textContent = errors[key].join(' ');
            }
        }
    }
    
    function showNotification(message, type = 'error') {
        notificationArea.textContent = message;
        notificationArea.className = `notification ${type}`;
        notificationArea.style.display = 'block';
    }

    async function loadCategories() {
        try {
            const response = await fetch(`${apiBaseUrl}/categories`);
            if (!response.ok) throw new Error('Kategoriler yüklenemedi.');
            
            const categories = await response.json();
            categorySelect.innerHTML = '<option value="">Choose a category</option>';
            categories.forEach(cat => {
                const option = document.createElement('option');
                option.value = cat.id;
                option.textContent = cat.name;
                categorySelect.appendChild(option);
            });
        } catch (error) {
            console.error(error);
            categorySelect.innerHTML = '<option value="">Categories could not be loaded</option>';
        }
    }

    form.addEventListener('submit', async (e) => {
        e.preventDefault();
        clearErrors();

        const formData = new FormData(form);
        const productData = Object.fromEntries(formData.entries());

        productData.price = parseFloat(productData.price);
        productData.stock = parseInt(productData.stock, 10);
        productData.categoryId = parseInt(productData.categoryId, 10);

        try {
            const response = await fetch(`${apiBaseUrl}/products`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(productData)
            });

            if (response.ok) {
                const newProduct = await response.json();
                sessionStorage.setItem('successMessage', `Product "${newProduct.name}" was added successfully!`);
                window.location.href = 'index.html';
            } else {
                const contentType = response.headers.get("content-type");
                let errorData;
                if (contentType && contentType.indexOf("application/json") !== -1) {
                    errorData = await response.json();
                } else {
                    errorData = await response.text();
                }

                if (errorData.errors) {
                    displayErrors(errorData.errors);
                } else {
                    showNotification(errorData);
                }
            }
        } catch (error) {
            console.error('Request error:', error);
            showNotification('An error occurred while connecting to the server.');
        }
    });

    loadCategories();
}); 