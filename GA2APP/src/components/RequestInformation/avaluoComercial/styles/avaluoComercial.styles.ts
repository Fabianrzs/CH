import { makeStyles } from "@material-ui/core/styles";

const useAvaluoComercialStyles = makeStyles(() => ({
  root: {
    background: "#fff",
    padding: 0,
    width: "100%",
  },

  header: {
    background: "#1f3f6d",
    color: "#fff",
    padding: "6px 12px",
    fontWeight: "bold",
    fontSize: 14,
  },

  subtitle: {
    fontWeight: "bold",
    fontSize: 13,
    padding: "6px 12px",
  },

  section: {
    background: "#d3d3d3",
    padding: "6px 8px",
    fontWeight: "bold",
    fontSize: 13,
    marginTop: 16,
    marginBottom: 12,
  },

  field: {
    marginBottom: 14,

    "& .MuiInputBase-root": {
      fontSize: 13,
    },

    "& .MuiInputLabel-root": {
      fontSize: 13,
    },
  },

  actions: {
    display: "flex",
    justifyContent: "flex-end",
    marginTop: 30,
    paddingBottom: 20,
  },

  saveButton: {
    background: "#163f92",
    color: "#fff",
    fontWeight: "bold",

    "&:hover": {
      background: "#0f2f70",
    },

    "&.Mui-disabled": {
      background: "#9e9e9e",
      color: "#fff",
    },
  },

  loadingClient: {
    display: "flex",
    alignItems: "center",
    marginBottom: 16,
  },

  loadingText: {
    marginLeft: 8,
  },
}));

export default useAvaluoComercialStyles;