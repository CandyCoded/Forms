test:
	echo "Error: no tests specified"

clean:
	git clean -xdf

deploy:
	git subtree push --prefix Assets/Plugins/CandyCoded.Forms origin upm

deploy-force:
	git push origin `git subtree split --prefix Assets/Plugins/CandyCoded.Forms master`:upm --force
