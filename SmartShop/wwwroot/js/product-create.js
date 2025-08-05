/**
 * Product Creation JavaScript Module
 * Handles product creation form functionality including preview, validation, and variants
 */

// Product Preview Module
const ProductPreview = {
    /**
     * Show product preview modal with current form data
     */
    show: function() {
        try {
            // Get form data with safe checking using ASP.NET Core rendered names
            const nameElement = $('#Product_Name');
            const priceElement = $('#Product_Price');
            const descriptionElement = $('#Product_Description');
            const imageElement = $('#Product_ImageUrl');
            const categoryElement = $('#Product_CategoryId');

            const productData = {
                name: nameElement.length ? (nameElement.val() || '').trim() || 'Chưa có tên' : 'Chưa có tên',
                price: priceElement.length ? (parseFloat(priceElement.val()) || 0) : 0,
                description: descriptionElement.length ? (descriptionElement.val() || '').trim() || 'Chưa có mô tả' : 'Chưa có mô tả',
                imageUrl: imageElement.length ? (imageElement.val() || '').trim() || 'https://via.placeholder.com/300x300?text=Không+có+hình+ảnh' : 'https://via.placeholder.com/300x300?text=Không+có+hình+ảnh',
                categoryId: categoryElement.length ? (categoryElement.val() || '0') : '0',
                categoryName: categoryElement.length ? (categoryElement.find('option:selected').text() || 'Chưa chọn danh mục') : 'Chưa chọn danh mục'
            };

            // Collect variants data
            const variants = this._collectVariants();

            // Validate basic data
            if (!productData.name || productData.name === 'Chưa có tên') {
                NotificationManager.showError('Vui lòng nhập tên sản phẩm trước khi xem trước');
                return;
            }

            if (productData.price <= 0) {
                NotificationManager.showError('Vui lòng nhập giá sản phẩm hợp lệ trước khi xem trước');
                return;
            }

            // Create and show modal
            this._createModal(productData, variants);
        } catch (error) {
            console.error('Error showing preview:', error);
            NotificationManager.showError('Có lỗi xảy ra khi tạo xem trước: ' + error.message);
        }
    },

    /**
     * Collect variants data from form
     * @returns {Array} Array of variant objects
     */
    _collectVariants: function() {
        const variants = [];
        try {
            $('#variants-container .variant-item').each(function() {
                const $variant = $(this);
                const colorInput = $variant.find('input[name*="Color"]');
                const sizeInput = $variant.find('input[name*="Size"]');
                const stockInput = $variant.find('input[name*="StockQuantity"]');
                
                const color = colorInput.length ? (colorInput.val() || '').trim() : '';
                const size = sizeInput.length ? (sizeInput.val() || '').trim() : '';
                const stockQuantity = stockInput.length ? (parseInt(stockInput.val()) || 0) : 0;
                
                if (color && size) {
                    variants.push({ 
                        color: color, 
                        size: size, 
                        stockQuantity: Math.max(0, stockQuantity) // Ensure non-negative
                    });
                }
            });
        } catch (error) {
            console.error('Error collecting variants:', error);
        }
        
        return variants;
    },

    /**
     * Create and display the preview modal
     * @param {Object} productData - Product information
     * @param {Array} variants - Product variants
     */
    _createModal: function(productData, variants) {
        try {
            const modalHtml = this._generateModalHtml(productData, variants);
            
            // Remove existing modal if any
            $('#productPreviewModal').remove();

            // Add modal to body and show
            $('body').append(modalHtml);
            
            // Ensure Bootstrap is loaded
            if (typeof bootstrap !== 'undefined') {
                const modal = new bootstrap.Modal(document.getElementById('productPreviewModal'));
                modal.show();
            } else if ($.fn.modal) {
                // Fallback to jQuery modal
                $('#productPreviewModal').modal('show');
            } else {
                NotificationManager.showError('Bootstrap modal not available');
                return;
            }

            // Clean up modal when closed
            $('#productPreviewModal').on('hidden.bs.modal', function () {
                $(this).remove();
            });
        } catch (error) {
            console.error('Error creating modal:', error);
            NotificationManager.showError('Có lỗi xảy ra khi tạo modal xem trước');
        }
    },

    /**
     * Generate HTML for the preview modal
     * @param {Object} productData - Product information
     * @param {Array} variants - Product variants
     * @returns {string} Modal HTML
     */
    _generateModalHtml: function(productData, variants) {
        const variantsHtml = variants.length > 0 ? 
            variants.map(v => `
                <div class="d-inline-block me-2 mb-2">
                    <span class="badge bg-secondary me-1">${v.color}</span>
                    <span class="badge bg-info me-1">${v.size}</span>
                    <span class="badge bg-warning text-dark">${v.stockQuantity} sẵn có</span>
                </div>
            `).join('') 
            : '<span class="text-muted">Chưa có biến thể nào</span>';

        const totalStock = variants.reduce((sum, v) => sum + v.stockQuantity, 0);

        return `
            <div class="modal fade" id="productPreviewModal" tabindex="-1" aria-labelledby="productPreviewModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="productPreviewModalLabel">
                                <i class="fas fa-eye me-2"></i>
                                Xem trước sản phẩm
                            </h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-5">
                                    <div class="text-center mb-3">
                                        <img src="${productData.imageUrl}" 
                                             alt="${productData.name}" 
                                             class="img-fluid rounded shadow"
                                             style="max-height: 300px; object-fit: cover;"
                                             onerror="this.src='https://via.placeholder.com/300x300?text=Lỗi+tải+hình+ảnh'">
                                    </div>
                                </div>
                                <div class="col-md-7">
                                    <h4 class="text-primary">${productData.name}</h4>
                                    <p class="text-muted mb-2">
                                        <i class="fas fa-tag me-1"></i>
                                        Danh mục: <span class="badge bg-primary">${productData.categoryName}</span>
                                    </p>
                                    <h5 class="text-success mb-3">
                                        <i class="fas fa-dollar-sign me-1"></i>
                                        ${productData.price.toLocaleString('vi-VN')} VND
                                    </h5>
                                    
                                    <div class="mb-3">
                                        <h6 class="fw-bold">Mô tả:</h6>
                                        <p class="text-muted">${productData.description}</p>
                                    </div>

                                    <div class="mb-3">
                                        <h6 class="fw-bold">Biến thể (${variants.length}):</h6>
                                        ${variantsHtml}
                                    </div>

                                    <div class="alert alert-info">
                                        <h6 class="fw-bold mb-2">
                                            <i class="fas fa-info-circle me-1"></i>
                                            Thống kê:
                                        </h6>
                                        <ul class="mb-0">
                                            <li>Tổng biến thể: <strong>${variants.length}</strong></li>
                                            <li>Tổng tồn kho: <strong>${totalStock}</strong></li>
                                            <li>Trạng thái: <strong class="text-success">Sẵn sàng tạo</strong></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
                                <i class="fas fa-times me-1"></i>
                                Đóng
                            </button>
                            <button type="button" class="btn btn-success" onclick="$('#productPreviewModal').modal('hide'); $('#productForm').submit();">
                                <i class="fas fa-save me-1"></i>
                                Lưu sản phẩm
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        `;
    }
};

// Notification Module
const NotificationManager = {
    /**
     * Show error notification
     * @param {string} message - Error message
     */
    showError: function(message) {
        this._createToast(message, 'danger', 'fas fa-exclamation-triangle');
    },

    /**
     * Show success notification
     * @param {string} message - Success message
     */
    showSuccess: function(message) {
        this._createToast(message, 'success', 'fas fa-check-circle success-icon');
    },

    /**
     * Show info notification
     * @param {string} message - Info message
     */
    showInfo: function(message) {
        this._createToast(message, 'info', 'fas fa-info-circle');
    },

    /**
     * Create and show toast notification
     * @param {string} message - Message to display
     * @param {string} type - Toast type (success, danger, info)
     * @param {string} iconClass - Icon CSS class
     */
    _createToast: function(message, type, iconClass) {
        const toast = $(`
            <div class="toast align-items-center text-white bg-${type} border-0" role="alert" aria-live="assertive" aria-atomic="true">
                <div class="d-flex">
                    <div class="toast-body">
                        <i class="${iconClass} me-2"></i>${message}
                    </div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
                </div>
            </div>
        `);
        
        this._ensureToastContainer();
        
        $('#toast-container').append(toast);
        toast.toast('show');
        
        toast.on('hidden.bs.toast', function () {
            $(this).remove();
        });
    },

    /**
     * Ensure toast container exists
     */
    _ensureToastContainer: function() {
        if ($('#toast-container').length === 0) {
            $('body').append('<div id="toast-container" class="toast-container position-fixed bottom-0 end-0 p-3"></div>');
        }
    }
};

// Global functions for backward compatibility
function previewProduct() {
    ProductPreview.show();
}

function showError(message) {
    NotificationManager.showError(message);
}

function showSuccess(message) {
    NotificationManager.showSuccess(message);
}

function showInfo(message) {
    NotificationManager.showInfo(message);
}
