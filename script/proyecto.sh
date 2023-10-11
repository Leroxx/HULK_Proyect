#! /bin/bash
echo Ingrese el comando run para inicializar el poyecto.
read command1

if [[ "$command1" == run ]] 
then
    echo "El poyecto ha sido iniciado."
    echo "Ingrese report para compilar el informe"
    echo "Ingrese slides para compilar la presentacion."
    echo "Ingrese show_report para abrir el informe."
    echo "Ingrese show_slides para abrir la presentacion."
    echo "Ingrese clean para limpiar los archivos generados al compilar"
    read command2
    case $command2 in
        report)
            cp ../informe_hulk/matcom.jpg ./
            pdflatex ../informe_hulk/informe.tex
            pdflatex ../informe_hulk/informe.tex
            ;;
        slides)
            pdflatex ../presentacion_hulk/presentacion.tex
            pdflatex ../presentacion_hulk/presentacion.tex
            pdflatex ../presentacion_hulk/presentacion.tex
            ;;
        show_report)
            echo "Escriba el gestor de pdf con el cual abrirlo si asi lo desea"
            read lector
            if [[ -e informe.pdf ]] 
            then
                if [[ $lector != "" ]]
                then
                $lector informe.pdf
                else
                xdg-open informe.pdf
                fi
            else
                cp ../informe_hulk/matcom.jpg ./
                pdflatex ../informe_hulk/informe.tex
                pdflatex ../informe_hulk/informe.tex
                if [[ $lector != "" ]]
                then
                $lector informe.pdf
                else
                xdg-open informe.pdf
                fi
            fi
            ;;
        show_slides)
            echo "Escriba el gestor de pdf con el cual abrirlo si asi lo desea"
            read lector
            if [[ -e presentacion.pdf ]] 
            then
                if [[ $lector != "" ]]
                then
                $lector presentacion.pdf
                else
                xdg-open presentacion.pdf
                fi
            else
                pdflatex ../presentacion_hulk/presentacion.tex
                pdflatex ../presentacion_hulk/presentacion.tex
                if [[ $lector != "" ]]
                then
                $lector presentacion.pdf
                else
                xdg-open presentacion.pdf
                fi
            fi
            ;;
        clean)
            rm *.aux *.log *.nav *.out *.pdf *.snm *.toc *.jpg *.vrb

    esac
else
    echo Ha ingresado el comando incorecto.
fi
