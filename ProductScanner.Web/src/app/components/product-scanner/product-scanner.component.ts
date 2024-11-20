import { Component, OnInit } from '@angular/core';
import { SignalRService } from '../../signalr.service';
import { HttpClient } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';


@Component({
  selector: 'app-product-scanner',
  templateUrl: './product-scanner.component.html',
  styleUrls: ['./product-scanner.component.css']
})
export class ProductScannerComponent implements OnInit {
  public products: any[] = [];
  public scannedBarcode: string = '';
  public updatedProduct: any = null;  // To hold the product being updated for animation
  public errorMessage: string = '';   // To hold any error message

  constructor(
    private signalRService: SignalRService, 
    private http: HttpClient,
    private snackBar: MatSnackBar // Inject MatSnackBar for showing error messages
  ) {}

  ngOnInit(): void {
    this.signalRService.startConnection();
    this.signalRService.listenForUpdates();
    this.getProducts();

    // Listen for updates from SignalR
    this.signalRService.productUpdate.subscribe((update) => {
      if (update) {
        // Update the product quantity in the local list
        const updatedProduct = this.products.find(p => p.barcode === update.barcode);
        if (updatedProduct) {
          updatedProduct.quantity = update.newQuantity;
          this.updatedProduct = updatedProduct;  // Store the updated product for animation
        }
      }
    });
  }

  // Fetch products from the backend
  getProducts() {
    this.http.get('/api/product').subscribe((data: any) => {
      this.products = data;
    });
  }

  // Handle the scanning of a product
  scanProduct() {
    if (this.scannedBarcode) {
      this.http.post(`/api/product/scan/${this.scannedBarcode}`, {})
        .subscribe(
          () => {
            this.getProducts();  // Refresh the product list after scanning
            this.scannedBarcode = '';  // Reset the barcode input after scanning
          },
          (error) => {
            // Handle error when product quantity is 0 or below
            if (error.status === 400) {
              this.errorMessage = error.error;  // Set the error message from the backend
              this.snackBar.open(this.errorMessage, 'Close', { duration: 3000 });  // Show the error in a Snackbar
            } else {
              console.error('Error scanning product', error);
            }
          }
        );
    }
  }
}
