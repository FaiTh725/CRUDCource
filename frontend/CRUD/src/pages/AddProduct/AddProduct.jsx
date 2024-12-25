import { useCallback, useEffect, useMemo, useState } from "react";
import SimpleButton from "../../components/buttons/simple_button/SimpleButton";
import FileInput from "../../components/inputs/file_input/FileInput";
import SimpleArea from "../../components/inputs/simple_area/SimpleArea";
import SimpleInput from "../../components/inputs/simple_input/Input";
import styles from "./AddProduct.module.css"
import ImageSlider from "../../components/ImageSlider/ImageSlider";
import axios from "axios";
import Cookies from "js-cookie";
import { useAuth } from "../../components/Auth/AuthContext";
import { useNavigate } from "react-router-dom";
import MiniMessages from "../../components/MiniMessages/MiniMessages";


const AddProduct = () => {
  const [addProductForm, setAddProductForm] = useState({
    productName: "",
    description: "",
    price: "",
    count: "",
    files: []
  });

  const [formError, setFormErrors] = useState({
    productNameError: "",
    descriptionError: "",
    priceError: "",
    countError: "",
    resultAddProduct: "",
  });

  const [modalIsActive, setModalIsActive] = useState(false);

  const auth = useAuth();
  const navigate = useNavigate();

  const handleFormChange = (e) => {
    const key = e.target.name;
    const newValue = e.target.value;

    setAddProductForm((prev) => ({
      ...prev,
      [key]: newValue
    }));
  }

  const handleChangeErrorsForm = (value, key) => {
    setFormErrors((prev) => ({
      ...prev,
      [key]: value
    }));
  }
  
  const images = useMemo(() => {
    return addProductForm.files.map(file => URL.createObjectURL(file));
  }, [addProductForm.files]);

  const setFiles = useCallback((files) => {
    setAddProductForm(prev => ({
      ...prev,
      files: [...files]
    }))
  }, []);


  useEffect(() => {
    
    setFormErrors({
      productNameError: "",
      descriptionError: "",
      priceError: "",
      countError: "",
      resultAddProduct: ""
    });

  }, [addProductForm])

  const handleAddProduct = async (e) => {
    e.preventDefault();

    let formIsValid = true;

    if(addProductForm.productName.trim() === "" ||
    addProductForm.productName.length > 30)
    {
      handleChangeErrorsForm("Product name should not be empty or greate than 30", "productNameError");
      formIsValid = false;
    }
    if(addProductForm.description.length > 300)
    {
      handleChangeErrorsForm("Description soo long, max 300", "descriptionError");
      formIsValid = false;
    }

    if(!(/[+-]?\d*\.?\d+/.test(addProductForm.price)))
    {
      handleChangeErrorsForm("Price is invalid format", "priceError");
      formIsValid = false;
    }
    
    if(!Number(addProductForm.count))
    {
      handleChangeErrorsForm("Number is invalid format or incorect value", "countError");
      formIsValid = false;
    }

    if(!formIsValid)
    {
      return;
    }

    if(!(auth || auth.user.email))
    {
      console.log("User is not registered");
      return;
    }

    const form = new FormData();
    form.append("Name", addProductForm.productName);
    form.append("SealerEmail", auth.user.email);
    form.append("Price", Number.parseFloat(addProductForm.price));
    form.append("Count", Number.parseInt(addProductForm.count));
    
    if(addProductForm.files && addProductForm.files[0])
    {
      addProductForm.files.forEach(file => {
        form.append("Files", file);
      })
    }
    
    if(addProductForm.description != "")
    {
      form.append("Description", addProductForm.description);
    }

    try
    {
      const response = await axios.post("https://localhost:7080/api/Product/Upload", 
        form,
        {
          headers: {
            'Content-Type': 'multipart/form-data',
            "Authorization": "Bearer " + Cookies.get("token")
          }
        }
      );

      if(response.data.statusCode === 0)
      {
        setAddProductForm({
          productName: "",
          description: "",
          price: "",
          count: "",
          files: []
        });

        handleChangeErrorsForm("Product Upload", "resultAddProduct");
        setModalIsActive(true);
      }
      else
      {
        handleChangeErrorsForm(response.data.description, "resultAddProduct");
        setModalIsActive(true);
      }

      console.log(response);
    }
    catch(error)
    {
      console.log(error);
      if(error.status === 401)
      {
        auth.logout();
        navigate("/account/login");
        return ;
      }

      console.log("Error Upload New Project");
    }
  }

  return (
    <div className={styles.AddProduct__Main}>
      <div className={styles.AddProduct__Wrapper}>
        <div className={styles.AddProduct__ProductFields}>
          <div className={styles.AddProduct__ImagesSliderWrapper}>
            <div className={styles.AddProduct__ImageSliderContainer}>
              <ImageSlider images={images}/>
            </div>
            <FileInput setFiles={setFiles}/>
          </div>
          <div className={styles.AddProduct__ProductData}>
            <SimpleInput labelName="Product Name" 
              value={addProductForm.productName} 
              name="productName" 
              action={handleFormChange}/>
            <span className={styles.AddProduct__ProductValidateError}>
              {formError.productNameError}
            </span>
            <SimpleArea labelName="Description" 
              value={addProductForm.description} 
              name="description" 
              action={handleFormChange}/>
            <span className={styles.AddProduct__ProductValidateError}>
              {formError.descriptionError}
            </span>
            <SimpleInput labelName="Price" 
              value={addProductForm.price} 
              name="price" 
              action={handleFormChange}/>
            <span className={styles.AddProduct__ProductValidateError}>
              {formError.priceError}
            </span>
            <SimpleInput labelName="Count" 
              value={addProductForm.count} 
              name="count" 
              action={handleFormChange}/>
            <span className={styles.AddProduct__ProductValidateError}>
              {formError.countError}
            </span>
          </div>
        </div>
        <div className={styles.AddProduct__Footer}>
          <SimpleButton name="Add" action={handleAddProduct}/>
        </div>
      </div>
      <MiniMessages message={formError.resultAddProduct} 
        isActive={modalIsActive} 
        setIsActive={setModalIsActive}/>
    </div>
  )
}

export default AddProduct;