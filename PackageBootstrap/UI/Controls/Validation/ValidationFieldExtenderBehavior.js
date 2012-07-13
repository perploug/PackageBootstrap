Type.registerNamespace('PackageBootstrap.UI.Controls.Validation');

PackageBootstrap.UI.Controls.Validation.ValidationFieldExtenderBehavior = function(element) {
    PackageBootstrap.UI.Controls.Validation.ValidationFieldExtenderBehavior.initializeBase(this, [element]);

    this._TargetLabelID = null;
    this._originalValidationMethod = null;
    this._validationMethodOverride = null;
    this._elementToValidate = null;
    this._InvalidTextBoxCssClass = null;
    this._ValidTextBoxCssClass = null;
    this._InvalidLabelCssClass = null;
    this._ValidLabelCssClass = null;
    this._focusAttached = false;
    this._isOpen = false;
    this._invalid = false;
    this._isBuilt = false;
    this._focusHandler = Function.createDelegate(this, this._onfocus);
}

PackageBootstrap.UI.Controls.Validation.ValidationFieldExtenderBehavior.prototype = {
    ///////////////////////
    // FUNCTIONS
    ///////////////////////
    initialize: function() {
        PackageBootstrap.UI.Controls.Validation.ValidationFieldExtenderBehavior.callBaseMethod(this, 'initialize');

        var elt = this.get_element();
        this._elementToValidate = $get(elt.controltovalidate);

        // Override the evaluation method of the current validator
        if (elt.evaluationfunction) {
            this._originalValidationMethod = Function.createDelegate(elt, elt.evaluationfunction);
            this._validationMethodOverride = Function.createDelegate(this, this._onvalidate);
            elt.evaluationfunction = this._validationMethodOverride;
        }
    },
    dispose: function() {
        PackageBootstrap.UI.Controls.Validation.ValidationFieldExtenderBehavior.callBaseMethod(this, 'dispose');
    },

    _onvalidate: function(val) {
        if (!this._originalValidationMethod(val)) {
            if (this._InvalidTextBoxCssClass) {
                if (this._elementToValidate != null) {
                    Sys.UI.DomElement.addCssClass(this._elementToValidate, this._InvalidTextBoxCssClass);
                    //}
                    //if(this._TargetLabelID != null)
                    //{
                    Sys.UI.DomElement.addCssClass(this._TargetLabelID, this._InvalidLabelCssClass);
                }
            }
            if (!this._focusAttached) {
                $addHandler(this._elementToValidate, "focus", this._focusHandler);
                this._focusAttached = true;
            }


            if (this._ValidTextBoxCssClass) {
                if (this._elementToValidate != null) {
                    Sys.UI.DomElement.removeCssClass(this._elementToValidate, this._ValidTextBoxCssClass);
                }
                if (this._TargetLabelID != null) {
                    Sys.UI.DomElement.removeCssClass(this._TargetLabelID, this._ValidLabelCssClass);
                }
            }
            
            
            this._invalid = true;
            return false;
        }
        else {
            if (this._InvalidTextBoxCssClass && this._invalid) {
                if (this._elementToValidate != null) {
                    Sys.UI.DomElement.removeCssClass(this._elementToValidate, this._InvalidTextBoxCssClass);
                }
                if (this._TargetLabelID != null) {
                    Sys.UI.DomElement.removeCssClass(this._TargetLabelID, this._InvalidLabelCssClass);
                }
            }

            Sys.UI.DomElement.addCssClass(this._elementToValidate, this._ValidTextBoxCssClass);
            Sys.UI.DomElement.addCssClass(this._TargetLabelID, this._ValidLabelCssClass);
            
            this._invalid = false;
            return true;
        }
    },
    _onfocus: function(e) {
        if (!this._originalValidationMethod(this.get_element())) {
            if (this._InvalidTextBoxCssClass) {

                Sys.UI.DomElement.addCssClass(this._elementToValidate, this._InvalidTextBoxCssClass);
            }
            if (this._InvalidLabelCssClass) {
                Sys.UI.DomElement.addCssClass(this._TargetLabelID, this._InvalidLabelCssClass);
            }
            return false;
        }
        else {
            return true;
        }
    },
    ///////////////////////
    // PROPERTIES
    ///////////////////////
    // TargetLabel
    get_TargetLabelID: function() {
        return this._TargetLabelID;
    },

    set_TargetLabelID: function(value) {
        this._TargetLabelID = value;
    },

    //InvalidTextBoxCssClass
    get_InvalidTextBoxCssClass: function() {
        return this._InvalidTextBoxCssClass;
    },

    set_InvalidTextBoxCssClass: function(value) {
        if (this._InvalidTextBoxCssClass != value) {
            this._InvalidTextBoxCssClass = value;
            this.raisePropertyChanged("InvalidTextBoxCssClass");
        }
    },

    //ValidTextBoxCssClass
    get_ValidTextBoxCssClass: function() {
        return this._ValidTextBoxCssClass;
    },

    set_ValidTextBoxCssClass: function(value) {
        if (this._ValidTextBoxCssClass != value) {
            this._ValidTextBoxCssClass = value;
            this.raisePropertyChanged("ValidTextBoxCssClass");
        }
    },

    //InvalidLabelCssClass
    get_InvalidLabelCssClass: function() {
        return this._InvalidLabelCssClass;
    },

    set_InvalidLabelCssClass: function(value) {
        if (this._InvalidLabelCssClass != value) {
            this._InvalidLabelCssClass = value;
            this.raisePropertyChanged("InvalidLabelCssClass");
        }
    },
    
    //ValidLabelCssClass
    get_ValidLabelCssClass: function() {
        return this._ValidLabelCssClass;
    },

    set_ValidLabelCssClass: function(value) {
        if (this._ValidLabelCssClass != value) {
            this._ValidLabelCssClass = value;
            this.raisePropertyChanged("ValidLabelCssClass");
        }
    }
}
PackageBootstrap.UI.Controls.Validation.ValidationFieldExtenderBehavior.registerClass('PackageBootstrap.UI.Controls.Validation.ValidationFieldExtenderBehavior', AjaxControlToolkit.BehaviorBase);
