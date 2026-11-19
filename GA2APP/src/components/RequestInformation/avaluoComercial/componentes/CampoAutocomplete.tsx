import React from "react";
import {
  CircularProgress,
  TextField,
} from "@material-ui/core";
import Autocomplete from "@material-ui/lab/Autocomplete";
import useAvaluoComercialStyles from "../styles/avaluoComercial.styles";

type Props = {
  label: string;
  options: any[];
  value: any | null;
  error?: string;
  loading?: boolean;
  disabled?: boolean;
  required?: boolean;
  noOptionsText?: string;
  loadingText?: string;
  getOptionLabel: (option: any) => string;
  getOptionSelected?: (
    option: any,
    selectedValue: any
  ) => boolean;
  onChange: (value: any | null) => void;
};

function CampoAutocomplete({
  label,
  options,
  value,
  error = "",
  loading = false,
  disabled = false,
  required = true,
  noOptionsText = "No se encontraron resultados",
  loadingText = "Cargando...",
  getOptionLabel,
  getOptionSelected,
  onChange,
}: Props) {
  const classes = useAvaluoComercialStyles();

  return (
    <Autocomplete
      fullWidth
      options={options}
      value={value}
      loading={loading}
      disabled={disabled}
      getOptionLabel={getOptionLabel}
      getOptionSelected={
        getOptionSelected
      }
      noOptionsText={noOptionsText}
      loadingText={loadingText}
      onChange={(
        _event: unknown,
        newValue: any | null
      ) => {
        onChange(newValue);
      }}
      renderInput={(params) => (
        <TextField
          {...params}
          className={classes.field}
          fullWidth
          required={required}
          label={label}
          error={Boolean(error)}
          helperText={error || ""}
          InputProps={{
            ...params.InputProps,
            endAdornment: (
              <>
                {loading ? (
                  <CircularProgress
                    size={18}
                  />
                ) : null}

                {
                  params.InputProps
                    .endAdornment
                }
              </>
            ),
          }}
        />
      )}
    />
  );
}

export default CampoAutocomplete;